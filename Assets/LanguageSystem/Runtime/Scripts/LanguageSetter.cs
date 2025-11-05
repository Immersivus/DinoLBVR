using UnityEngine;
using UnivrseSdk.Services;

namespace Univrse.Studio.LanguageSystemV2 
{
    public class LanguageSetter : MonoBehaviour
    {
        [SerializeField] private LanguageObject _language;

        private ILanguageService _languageService;

        private void Start()
        {
            _languageService = ServiceLocator.Instance.GetService<ILanguageService>();
        }

        public void SetLanguage()
        {
            _languageService.SetLanguage(_language);
        }
    }
}