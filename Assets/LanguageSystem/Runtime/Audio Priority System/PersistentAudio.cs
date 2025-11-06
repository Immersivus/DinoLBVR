using UnityEngine;
using UnityEngine.SceneManagement;
using Univrse.Studio.LanguageSystemV2;

namespace Univrse.Studio.AudioPrioritySystem
{

    [RequireComponent(typeof(AudioLauncher))]
    public class PersistentAudio : MonoBehaviour
    {
        private AudioLauncher _languageAudio;
        private bool _isPersistent;
        private bool _audioFinished;
        private Transform _originalParent;
        private Scene _audioOriginalScene;

        private void Awake()
        {
            _languageAudio = GetComponent<AudioLauncher>();
            _originalParent = transform.parent;
            _audioOriginalScene = gameObject.scene;
            _languageAudio.OnAudioPlayActions += delegate { KeepObjectPersistent(); };
            _languageAudio.OnAudioFinishActions += delegate { HandleFinishAudio(); };
            _languageAudio.OnAudioStopActions += delegate { HandleFinishAudio(); };


        }

        private void OnEnable()
        {
            if (_languageAudio.IsPlaying())
            {
                KeepObjectPersistent();
            }
        }

        private void Update()
        {
            if (_isPersistent) { 
                FollowOriginalParent();
            }
        }

        private void KeepObjectPersistent()
        {
            if (!_isPersistent)
            {
                _isPersistent = true;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void FollowOriginalParent()
        {
            if (_originalParent != null)
            {
                transform.position = _originalParent.position;
                transform.rotation = _originalParent.rotation;
            }
        }

        private void HandleFinishAudio()
        {
            if (_audioOriginalScene.isLoaded) { 
                ResetOriginalParent();
            }
            else
            {
                DestroyObject();
            }
        }

        private void DestroyObject()
        {
            Destroy(gameObject);
        }

        private void ResetOriginalParent()
        {
            RemoveFromDontDestroyOnLoad(gameObject);
            if (_originalParent != null) { 
                transform.parent = _originalParent;
            }
        }

        private void RemoveFromDontDestroyOnLoad(GameObject obj)
        {
            SceneManager.MoveGameObjectToScene(obj, _audioOriginalScene);
            _isPersistent = false;
        }

        private void OnDestroy()
        {
            _languageAudio.OnAudioPlayActions -= delegate { KeepObjectPersistent(); };
            _languageAudio.OnAudioFinishActions -= delegate { HandleFinishAudio(); }; 
            _languageAudio.OnAudioStopActions -= delegate { HandleFinishAudio(); };

        }
    }
}