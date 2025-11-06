using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnivrseSdk.Services;

namespace Univrse.Studio.LanguageSystemV2
{
    public class LanguageTrigger : MonoBehaviour
    {
        [SerializeField] LanguageObject _language;
        [SerializeField] public UnityEvent OnSelect;
        [SerializeField] public UnityEvent OnDiselect;

        public static LanguageTrigger _currentLanguageTrigger;
        private ILanguageService _languageService;
        private bool _isActive = false;

        private void Start()
        {
            _languageService = ServiceLocator.Instance.GetService<ILanguageService>();
            if (_languageService == null) return;
            if (_language != _languageService.CurrentLanguage) Deactivate();
        }

        public void ActivateLanguageTrigger()
        {
            Activate(false);
        }

        [ContextMenu("Activate")]
        void Activate(bool _isInit)
        {
            if (_isActive) return;

            if (_currentLanguageTrigger)
            {
                _currentLanguageTrigger.Deactivate();
            }

            _isActive = true;
            _currentLanguageTrigger = this;
            if (_languageService == null) _languageService.SetLanguage(_language);
            if (!_isInit) OnSelect?.Invoke();
        }

        public void Deactivate()
        {
            _isActive = false;
            OnDiselect?.Invoke();
        }
    }
} 

