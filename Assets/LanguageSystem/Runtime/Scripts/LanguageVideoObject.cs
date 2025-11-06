#if AVPRO
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace Univrse.Studio.LanguageSystemV2
{
    [CreateAssetMenu(menuName = "Univrse/Language System/Video Object", order = 2)]
    public class LanguageVideoObject : ScriptableObject
    {
        public List<LanguageVideoData> data = new List<LanguageVideoData>();
    }
}
#endif
