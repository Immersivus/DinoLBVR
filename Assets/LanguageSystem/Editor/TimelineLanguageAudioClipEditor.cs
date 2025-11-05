using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEditor.Timeline;

#if UNITY_EDITOR
namespace Univrse.Studio.LanguageSystemV2
{
    [CustomTimelineEditor(typeof(TimelineLanguageAudioAsset))]
    public class TimelineLanguageAudioClipEditor : ClipEditor
    {

        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            base.DrawBackground(clip, region);
            var asset = clip.asset as TimelineLanguageAudioAsset;
            if (!asset || !asset.LanguageObject) return;

            var iconSize = new Vector2(18, 18);

            asset.OnSetTime += (x) => clip.duration = x;

            var backgroundRegion = new Rect(
                region.position.position.x,
                region.position.position.y,
                region.position.width,
                region.position.height);

            float maxLength = 0;

            foreach (var item in asset.LanguageObject.data)
            {
                if (item.clip && item.clip.length > maxLength) maxLength = item.clip.length;
            }

            clip.displayName = asset.LanguageObject.name;

            var color = maxLength > clip.duration ? Color.red : Color.green;
            if (!asset.MatchAudioWithTimeline) color = Color.blue;
            EditorGUI.DrawRect(backgroundRegion, color);
        }
    }
}
#endif