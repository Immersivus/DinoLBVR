using System;
using Univrse.Studio.LanguageSystemV2;

namespace UnivrseSdk.Services
{
    public class LanguageManager: ILanguageService
    {
        public event Action<LanguageObject> OnCurrentLanguageChange;
        private LanguageObject _currentLanguage;

        public LanguageManager(LanguageConfig languageConfig)
        {
            SetLanguage(languageConfig.DefaultLanguage);
        }

        public LanguageObject CurrentLanguage { get => _currentLanguage;}

        public void SetLanguage(LanguageObject _language)
        {
            if (_currentLanguage == _language) return;
            _currentLanguage = _language;
            OnCurrentLanguageChange?.Invoke(_currentLanguage);
        }
    }
}

