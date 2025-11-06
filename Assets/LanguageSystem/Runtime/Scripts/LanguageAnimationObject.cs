using UnityEngine;
using System.Collections.Generic;

namespace Univrse.Studio.LanguageSystemV2
{
    [CreateAssetMenu(menuName = "Univrse/Language System/Animation Object", order = 1)]
    public class LanguageAnimationObject : ScriptableObject
    {
        public List<LanguageAnimationData> data = new List<LanguageAnimationData>();
    }
}
