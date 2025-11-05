#if AVPRO
using RenderHeads.Media.AVProVideo;
using System;
using UnityEngine;
using UnivrseSdk.Services;

namespace Univrse.Studio.LanguageSystemV2
{
    [Serializable]
    public class LanguageVideoData
    {
        public LanguageObject language;
        public string videoPath;
    }

    public class LanguageVideo : MonoBehaviour
    {
        [SerializeField] LanguageVideoObject languages;
        [SerializeField] MediaPlayer mediaPlayer;
        [SerializeField] bool playOnAwake;
        private ILanguageService _languageService;



        private void Start()
        {
            _languageService = ServiceLocator.Instance.GetService<ILanguageService>();
            _languageService.OnCurrentLanguageChange += OnCurrentLanguageChange;
            SetVideoByLanguage(_languageService.CurrentLanguage);

            if (playOnAwake) 
            {
                mediaPlayer.OpenMedia(true);
                mediaPlayer.Play();
                
            }
        }

        private void OnDestroy()
        {
            if (_languageService != null) _languageService.OnCurrentLanguageChange -= OnCurrentLanguageChange;
        }

        private void OnCurrentLanguageChange(LanguageObject language)
        {
            SetVideoByLanguage(language);
        }

        void SetVideoByLanguage(LanguageObject language)
        {
            LanguageVideoData data = languages.data.Find(x => x.language == language);
            if (data == null || data.videoPath == "") return;
            mediaPlayer.OpenMedia(MediaPathType.RelativeToPersistentDataFolder, data.videoPath, false);
        }
    }
}
#endif
