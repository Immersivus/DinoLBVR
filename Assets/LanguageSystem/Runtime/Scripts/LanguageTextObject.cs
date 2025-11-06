using UnityEngine;
using System.Collections.Generic;

namespace Univrse.Studio.LanguageSystemV2
{
    [CreateAssetMenu(menuName = "Univrse/Language System/Text Object", order = 2)]
    public class LanguageTextObject : ScriptableObject
    {
        public List<LanguageTextData> data = new List<LanguageTextData>();
    }
}
