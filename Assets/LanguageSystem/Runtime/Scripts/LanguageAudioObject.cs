using UnityEngine;
using System.Collections.Generic;

namespace Univrse.Studio.LanguageSystemV2
{
    [CreateAssetMenu(menuName = "Univrse/Language System/Audio Object", order = 1)]
    public class LanguageAudioObject : ScriptableObject
    {
        public List<LanguageAudioData> data = new List<LanguageAudioData>();
    }
}
