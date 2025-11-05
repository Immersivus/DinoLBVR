using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Univrse.Studio.AudioPrioritySystem
{
    [Serializable]
    public class AudioSourcePriorityObject
    {
        [SerializeField] private float _fadeOutTime;
        [HideInInspector] public AudioSource AudioSource;
        public AudioLayer Priority;

        public event Action<AudioSourcePriorityObject> OnAudioPlay = delegate { };
        public event Action<AudioSourcePriorityObject> OnAudioFinish = delegate { };
        public event Action<AudioSourcePriorityObject> OnAudioStop = delegate { };

        private float _startedVolume;
        private bool _playing;
        private bool _isWaitingToPlay;
        private CancellationTokenSource _cancellationTokenSource;
        
        public bool Playing { get => _playing; }
        public float FadeOutTime { get => _fadeOutTime; }
        public bool IsWaitingToPlay { get => _isWaitingToPlay; set => _isWaitingToPlay = value; }

        public void Play()
        {
            if (AudioSource != null)
            {
                ResetAudioSourceVolume();
                AudioSource.Play();
                RestartAudioFinishCheck();
                _playing = true;
                OnAudioPlay.Invoke(this);
            }
        }

        public void Stop()
        {
            if (AudioSource != null)
            {
                AudioSource.Stop();
                _playing = false;
                OnAudioStop.Invoke(this);
                CancelAudioFinishCheck();
            }
        }

        public void Pause()
        {
            if (AudioSource != null)
            {
                AudioSource.Pause();
                _playing = false;
                CancelAudioFinishCheck();
            }
        }

        public void SetAudioClip(AudioClip clip)
        {
            if (AudioSource != null)
            {
                AudioSource.clip = clip;
            }
        }

        public void FadeOut(Action onFadeOutFinish)
        {
            if (AudioSource != null)
            {
                _startedVolume = AudioSource.volume;
                FadeOutAsync(onFadeOutFinish);
            }
        }

        private void ResetAudioSourceVolume()
        {
            AudioSource.volume = 1;
        }

        public float GetAudioSourceTime()
        {
            return AudioSource.time;
        }

        public void SetAudioSourceTime(float value)
        {
            AudioSource.time = value;
        }

        private async void FadeOutAsync(Action onFadeOutFinish)
        {
            if (AudioSource == null || _fadeOutTime <= 0) return;

            float startVolume = AudioSource.volume;
            float elapsedTime = 0f;
            while (elapsedTime < _fadeOutTime)
            {
                elapsedTime += Time.deltaTime;
                SetAudioVolume(Mathf.Lerp(startVolume, 0, elapsedTime / _fadeOutTime));
                await Task.Yield();
            }
            SetAudioVolume(0);
            Stop();
            onFadeOutFinish.Invoke();
        }

        private void SetAudioVolume(float volume)
        {
            if (AudioSource != null)
            {
                AudioSource.volume = volume;
            }
        }

        private void RestartAudioFinishCheck()
        {
            CancelAudioFinishCheck();
            _cancellationTokenSource = new CancellationTokenSource();
            CheckAudioFinish(_cancellationTokenSource.Token);
        }

        private void CancelAudioFinishCheck()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async void CheckAudioFinish(CancellationToken cancellationToken)
        {
            if (AudioSource == null || AudioSource.clip == null) return;

            float audioClipTime = AudioSource.clip.length;
            float elapsedTime = 0f;

            while (elapsedTime < audioClipTime)
            {
                if (cancellationToken.IsCancellationRequested)
                    return; 

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                OnAudioFinish.Invoke(this);
                _playing = false;
            }
        }
    }
}
