using System;
using TMPro;
using UnityEngine;
using UnivrseSdk.Services;

namespace Univrse.Studio.LanguageSystemV2
{
    [Serializable]
    public class LanguageTextData
    {
        public LanguageObject language;
        public string text;
    }

    public class LanguageText: MonoBehaviour
    {
        [SerializeField] private LanguageTextObject languages;

        private ILanguageService _languageService;

        private void Start()
        {                   
            _languageService = ServiceLocator.Instance.GetService<ILanguageService>();
            _languageService.OnCurrentLanguageChange += OnCurrentLanguageChange;
            SetTextByLanguage(_languageService.CurrentLanguage);
        }

        private void OnCurrentLanguageChange(LanguageObject language)
        {
            SetTextByLanguage(language);
        }

        private void SetTextByLanguage(LanguageObject language)
        {
            LanguageTextData data = languages.data.Find(x => x.language == language);
            if (data == null || data.text== "") return;
           
            if(GetComponent<TextMeshPro>()) GetComponent<TextMeshPro>().text = data.text;
            else if(GetComponent<TextMeshProUGUI>()) GetComponent<TextMeshProUGUI>().text = data.text;
        }

        private void OnDestroy()
        {
            _languageService.OnCurrentLanguageChange -= OnCurrentLanguageChange;
        }
    }
}
