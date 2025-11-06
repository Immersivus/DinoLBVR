using UnityEngine;
using System.Collections.Generic;

namespace Univrse.Studio.LanguageSystemV2
{
    [CreateAssetMenu(menuName = "Univrse/Language System/Texture Object", order = 2)]
    public class LanguageTextureObject : ScriptableObject
    {
        public List<LanguageTextureData> data = new List<LanguageTextureData>();
    }
}
