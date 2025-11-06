using System;
using Univrse.Studio.LanguageSystemV2;

namespace UnivrseSdk.Services
{
    public interface ILanguageService
    {
        public LanguageObject CurrentLanguage { get; }
        public event Action<LanguageObject> OnCurrentLanguageChange;
        public void SetLanguage(LanguageObject language);
    }
}
