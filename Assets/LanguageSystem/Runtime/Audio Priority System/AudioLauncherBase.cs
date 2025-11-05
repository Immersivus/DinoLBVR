using System;
using UnityEngine;
using UnityEngine.Events;
using UnivrseSdk.Services;

namespace Univrse.Studio.AudioPrioritySystem
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AudioLauncherBase : MonoBehaviour
    {
        [SerializeField] protected AudioSourcePriorityObject _audioObject;
        [SerializeField] private UnityEvent _onAudioFinish = new UnityEvent();

        private IAudioService _audioService;
        public AudioSource AudioSource { get => _audioObject.AudioSource; }

        public event Action<AudioLauncherBase> OnAudioPlayActions = delegate { };
        public event Action<AudioLauncherBase> OnAudioFinishActions = delegate { };
        public event Action<AudioLauncherBase> OnAudioStopActions = delegate { };


        protected virtual void Start()
        {
            if (_audioObject.AudioSource == null) TryGetComponent(out _audioObject.AudioSource);
            _audioService = ServiceLocator.Instance.GetService<IAudioService>();
            _audioObject.OnAudioPlay += OnAudioPlay;
            _audioObject.OnAudioStop += OnAudioStop;
        }


        public void Play()
        {
            Debug.Log("Play");
            _audioService.PlayAudio(_audioObject);
            _audioObject.OnAudioFinish += OnAudioFinish;
        }
        private void OnAudioPlay(AudioSourcePriorityObject audioSourcePriorityObject)
        {
            OnAudioPlayActions.Invoke(this);
        }

        private void OnAudioFinish(AudioSourcePriorityObject audioSourcePriorityObject)
        {
            OnAudioFinishActions.Invoke(this);
            _onAudioFinish.Invoke();
            _audioObject.OnAudioFinish -= OnAudioFinish;

        }

        private void OnAudioStop(AudioSourcePriorityObject audioSourcePriorityObject)
        {
            OnAudioStopActions.Invoke(this);
        }

        public void PlayIgnoringLayer()
        {
            _audioObject.Play();
        }

        public void Stop()
        {
            _audioObject.Stop();
        }

        public void SetAudioClip(AudioClip clip)
        {
            _audioObject.AudioSource.clip = clip;
        }

        public AudioClip GetAudioClip()
        {
            return _audioObject.AudioSource.clip;
        }

        public void SetTime(float time)
        {
            _audioObject.SetAudioSourceTime(time);
        }

        public float GetTime()
        {
            return _audioObject.GetAudioSourceTime();
        }

        public bool IsPlaying()
        {
            return _audioObject.Playing;
        }

        public float GetLength()
        {
            if (_audioObject.IsWaitingToPlay)
            {
                return _audioObject.AudioSource.clip.length + _audioObject.FadeOutTime;
            }
            else
            {
                return _audioObject.AudioSource.clip.length;
            }
        }

        private void OnDestroy()
        {
            _audioObject.OnAudioPlay -= OnAudioPlay;
            _audioObject.OnAudioStop -= OnAudioStop;
        }
    }
}
