using System;
using UnityEngine;
using UnityEngine.Playables;
using XRoam.CrossReference.References;

namespace Univrse.Studio.LanguageSystemV2
{
    public class TimelineLanguageAudioAsset : PlayableAsset
    {
        [SerializeField] private GuidReference _audioReference;

        public Action<float> OnSetTime;
        public LanguageAudioObject LanguageObject;
        
        [Tooltip ("TRUE\n- Audio time will match with TimelineClip time\n- If timeline is paused, audio will be paused too. \n- The length of the TimelineClip should be as longer than longer AudioClip on languages \n\nFALSE\n- This TimelineClip just will Play the Audio")]
        public bool MatchAudioWithTimeline;
        public double Start;
        public double End;

        public GuidReference AudioReference { get => _audioReference;}

        public override Playable CreatePlayable(PlayableGraph _graph, GameObject _owner)
        {
            ScriptPlayable<TimelineLanguageAudioBehaviour> playable = ScriptPlayable<TimelineLanguageAudioBehaviour>.Create(_graph);
            TimelineLanguageAudioBehaviour behaviour = playable.GetBehaviour();
            behaviour.Asset = this;           
            return playable;
        }
    }
}
