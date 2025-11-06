using Univrse.Studio.AudioPrioritySystem;

namespace UnivrseSdk.Services
{
    public class AudioPriorityController: IAudioService
    {
        private AudioSourcePriorityObject _currentAudio = new AudioSourcePriorityObject();

        public void PlayAudio(AudioSourcePriorityObject audioSourcePriority)
        {
            HandleAudioPlay(audioSourcePriority);
        }

        private void HandleAudioPlay(AudioSourcePriorityObject audioSourcePriority)
        {
            if (audioSourcePriority.Priority == AudioLayer.IgnoreLayer)
            {
                PlayNewAudio(audioSourcePriority);
                return;
            }
            if (_currentAudio.Playing && _currentAudio != audioSourcePriority) { 
                SwitchAudio(audioSourcePriority);
            }
            else if(!_currentAudio.Playing)
            {
                PlayNewAudio(audioSourcePriority);
            }
        }

        private bool HasPriority(AudioSourcePriorityObject newAudio, AudioSourcePriorityObject currentAudio)
        {
            return newAudio.Priority >= currentAudio.Priority;
        }

        private void SwitchAudio(AudioSourcePriorityObject audioSourcePriority)
        {
            if(HasPriority(audioSourcePriority, _currentAudio))
            {
                _currentAudio.FadeOut(delegate { PlayNewAudio(audioSourcePriority); });
                audioSourcePriority.IsWaitingToPlay = true;
            }
        }

        private void PlayNewAudio(AudioSourcePriorityObject audioSourcePriority)
        {
            audioSourcePriority.Play();
            _currentAudio = audioSourcePriority;
        }
    }
}