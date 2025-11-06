using UnityEngine;
using Univrse.Studio.LanguageSystemV2;

namespace UnivrseSdk.Services
{
    public class Installer : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private LanguageConfig _languageConfig;

        private void Awake()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            ServiceLocator services = ServiceLocator.Instance;
            services.RegisterService<ILanguageService>(new LanguageManager(_languageConfig));
            services.RegisterService<IAudioService>(new AudioPriorityController());
        }
    }
}
