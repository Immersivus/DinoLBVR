using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Univrse.Studio.LanguageSystemV2
{
#if UNITY_EDITOR
    [CustomEditor(typeof(TimelineLanguageAudioAsset))]
    public class TimelineLanguageAudioAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TimelineLanguageAudioAsset myScript = (TimelineLanguageAudioAsset)target;
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("CLIPS TIMES", EditorStyles.boldLabel);
            
            foreach (var item in myScript.LanguageObject.data)
            {
                if (item.language != null && item.clip != null) 
                {
                    EditorGUILayout.LabelField($"{item.language.name} AudioClip Length: {item.clip.length} seconds");
                }
            }

            if (GUILayout.Button("Set Length from longest AudioClip"))
            {
                SetTimelineClipLength(myScript, myScript.LanguageObject.data.Max(x => x.clip.length));
            }

        }

        public void SetTimelineClipLength(TimelineLanguageAudioAsset timelineLanguageAudioAsset, float newDuration)
        {
            timelineLanguageAudioAsset.OnSetTime?.Invoke(newDuration);
        }
    }
#endif
}
