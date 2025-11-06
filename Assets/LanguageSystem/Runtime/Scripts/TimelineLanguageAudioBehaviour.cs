using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using Univrse.Studio.AudioPrioritySystem;
using UnivrseSdk.Services;
using XRoam.CrossReference;

namespace Univrse.Studio.LanguageSystemV2
{
    [System.Serializable]
    public class TimelineLanguageAudioBehaviour : PlayableBehaviour
    {
        private const int IndexDefaultRootPlayable = 0;
        private const float AudioSyncTolerance = 0.1f;
        private GameObject _guidObject;
        private GuidManagerService _guidManagerService;

        public TimelineLanguageAudioAsset Asset;

        private ILanguageService _languageService;
        private AudioLauncherBase _audioLauncher;
        private bool _started;
        private bool _pause;
        private bool _isPlaying;
        private bool _finished;
        

        public void Init()
        {
            _languageService = ServiceLocator.Instance.GetService<ILanguageService>();
            _guidManagerService = GuidManagerService.Instance;
            TryInitializeData();
        }

        private void TryInitializeData()
        {
            if (!CheckCrossReferences()) return;
                SetGuidObject();
                SetGuidAudioLauncher();
        }

        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
            PauseAudio();
        }

        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
            if (Asset.MatchAudioWithTimeline)
            {
                PauseAudio();
            }
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            if (Application.isPlaying)
            {
                Init();
                if (!_audioLauncher && _guidObject != null)
                {
                    _audioLauncher = GetAudioSource();
                }

                _finished = false;
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            if (!_isPlaying)
            {
                _isPlaying = true;
            }
            else if(!_finished)
            {
                PlayLanguageAudio(playable);
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);

            if (!Application.isPlaying) return;

            if (TimelineHasFinished(playable) || HasTrackFinished(playable, info))
            {
                HandleTimelineEnd();
            }

            if (Asset.MatchAudioWithTimeline)
            {
                PauseAudio();
            }

            _isPlaying = false;
        }

        private void PlayLanguageAudio(Playable playable)
        {
            if (!LanguageAudioInitialized() || _audioLauncher == null) return;

            if (!TryGetAudioClip<AudioClip>(out AudioClip audioClip)) return;

            if (Asset.MatchAudioWithTimeline)
            {
                HandleAudioSync(playable);

                if (TimelineHasFinished(playable))
                {
                    StopAudioSourceObject();
                    return;
                }
            }

            if (!AudioClipHasStarted())
            {
                PlayAudioClip(audioClip);
                if (!Asset.MatchAudioWithTimeline)
                {
                    _audioLauncher.OnAudioFinishActions += StopAudioSourceObject;
                }
            }
        }

        private void HandleTimelineEnd()
        {
            _started = false;

            if (Asset.MatchAudioWithTimeline)
            {
                StopAudioSourceObject();
            }
        }

        private void PauseAudio()
        {
            if (_audioLauncher != null)
            {
                _audioLauncher.Stop();
            }
        }

        private AudioLauncher GetAudioSource()
        {
            return _guidObject.GetComponent<AudioLauncher>();
        }

        private void HandleAudioSync(Playable playable)
        {
            if (_audioLauncher.GetAudioClip() == null) return;

            if (IsAudioOutOfSync(playable))
            {
                _audioLauncher.SetTime((float)playable.GetTime());
            }
        }

        private bool LanguageAudioInitialized()
        {
            return (_languageService != null) && Asset && _languageService.CurrentLanguage;
        }

        private bool TryGetAudioClip<AudioClip>(out UnityEngine.AudioClip component)
        {
            component = Asset.LanguageObject.data.Find(x => x.language == _languageService.CurrentLanguage).clip;
            return component != null;
        }

        private bool TimelineHasFinished(Playable playable)
        {
            double currentTime = playable.GetGraph().GetRootPlayable(IndexDefaultRootPlayable).GetTime();
            double timelineTotalTime = playable.GetGraph().GetRootPlayable(IndexDefaultRootPlayable).GetDuration();

            return currentTime >= timelineTotalTime;
        }

        private bool HasTrackFinished(Playable playable, FrameData info)
        {
            double duration = playable.GetDuration();
            double count = playable.GetTime() + info.deltaTime;
            return info.effectivePlayState == PlayState.Paused && count > duration;
        }

        private bool IsAudioOutOfSync(Playable playable)
        {
            float timelineTime = (float)playable.GetTime();
            float audioTime = _audioLauncher.GetTime();

            bool isDesynchronized = Mathf.Abs(audioTime - timelineTime) > AudioSyncTolerance;
            bool isWithinAudioClip = timelineTime < _audioLauncher.GetAudioClip().length;

            return isDesynchronized && isWithinAudioClip;
        }

        private bool AudioClipHasStarted()
        {
            return _started && _audioLauncher.IsPlaying();
        }

        private void PlayAudioClip(AudioClip audioClip)
        {
            _started = true;
            _audioLauncher.SetAudioClip(audioClip);
            if (Asset.MatchAudioWithTimeline)
            {
                _audioLauncher.PlayIgnoringLayer();
            }
            else
            {
                _audioLauncher.Play();
            }
        }

        private void SetGuidObject()
        {
            _guidObject = _guidManagerService.FindById(Asset.AudioReference.Guid);

            if (_guidObject == null)
                Debug.LogError($"[SyncAudioLauncher] The Cross Reference is null");
        }

        private void SetGuidAudioLauncher()
        {
            if (!_guidObject.TryGetComponent(out _audioLauncher))
                Debug.LogError($"[SyncAudioLauncher] The Audio Launcher is null");
        }

        private bool CheckCrossReferences()
        {
            if (Asset.AudioReference is not { IsActiveScene: true })
                return false;

            return true;
        }

        async void DestroyAudioSourceOnFinishAudio()
        {
            int timeToDestroy = (int)(_audioLauncher.GetLength() * 1000);
            await Task.Delay(timeToDestroy);
            
            StopAudioSourceObject();
        }

        private void StopAudioSourceObject(AudioLauncherBase previousAudioLauncher)
        {
            if(_audioLauncher == previousAudioLauncher && !_finished)
            {
                if (previousAudioLauncher != null)
                {
                    _finished = true;
                    previousAudioLauncher.Stop();
                }
            }
        }

        private void StopAudioSourceObject()
        {
            if (_finished) return;
            if (_audioLauncher != null)
            {
                 _finished = true;
                _audioLauncher.Stop();
            }
        }
    }
}
