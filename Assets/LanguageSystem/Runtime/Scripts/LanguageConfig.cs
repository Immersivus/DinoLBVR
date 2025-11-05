using UnityEngine;

namespace Univrse.Studio.LanguageSystemV2
{
    [CreateAssetMenu(menuName = "Univrse/Language System/LanguageConfig", order = 0)]
    public class LanguageConfig : ScriptableObject
    {
        public LanguageObject DefaultLanguage;
    }
}
