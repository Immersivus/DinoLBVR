using System;
using UnityEngine;
using UnivrseSdk.Services;

namespace Univrse.Studio.LanguageSystemV2 
{
    [Serializable]
    public class LanguageTextureData
    {
        public LanguageObject language;
        public Texture texture;
    }

    public class LanguageTexture : MonoBehaviour
    {
        [SerializeField] string parameterToChange;
        [SerializeField] LanguageTextureObject languages;

        private ILanguageService _languageService;

        private void Start()
        {
            _languageService = ServiceLocator.Instance.GetService<ILanguageService>();
            _languageService.OnCurrentLanguageChange += OnCurrentLanguageChange;
            SetTextureByLanguage(_languageService.CurrentLanguage);
        }

        private void OnCurrentLanguageChange(LanguageObject language)
        {
            SetTextureByLanguage(language);
        }

        private void SetTextureByLanguage(LanguageObject language)
        {
            LanguageTextureData data = languages.data.Find(x => x.language == language);

            if (data.texture) GetComponent<Renderer>().material.SetTexture(parameterToChange, data.texture);
        }

        private void OnDestroy()
        {
            _languageService.OnCurrentLanguageChange -= OnCurrentLanguageChange;
        }
    }
}

