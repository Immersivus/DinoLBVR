using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine;
using System.ComponentModel;


namespace Univrse.Studio.LanguageSystemV2
{
    [DisplayName("Univrse/Language Audio Track")]
    [TrackClipType(typeof(TimelineLanguageAudioAsset))]
    public class TimelineLanguageAudioTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var clip in GetClips())
            {
                var myAsset = clip.asset as TimelineLanguageAudioAsset;
                if (myAsset)
                {
                    myAsset.Start = clip.start;
                    myAsset.End = clip.end;
                }
            }

            return base.CreateTrackMixer(graph, go, inputCount);
        }
    }
}