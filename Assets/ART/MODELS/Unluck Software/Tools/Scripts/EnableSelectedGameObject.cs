namespace UnluckSoftware
{
	using UnityEngine;

#if UNITY_EDITOR
	using UnityEditor;
#endif

	[ExecuteAlways]
	public class EnableSelectedGameObject :MonoBehaviour
	{
		[HideInInspector] public Transform prevSelect;
		[HideInInspector] public Transform prevSelectWrong;
		[Header("! Gizmos no longer required.")]
		public int particleSystems;
		public bool disableMe;
		public string ignoreTag = "^^^^";
		public bool disableOnPlay;
		public bool autoSlideShow;
		public float autoSlideShowDelay = 3f;
		public string nextKey = "n";
		public bool randomEnable = false;
		public bool repeat;

		int emitOnEnable = 1;
		int counter = 0;
		Transform randomEnablePlaceHolder;
		GameObject prevRandomBird;
		bool autoSlideShowStarted;
		bool selfSelected;

#if UNITY_EDITOR
		private static EnableSelectedGameObject activeInstance;

		[InitializeOnLoadMethod]
		private static void Init()
		{
			EditorApplication.update -= EditorUpdate;
			EditorApplication.update += EditorUpdate;
		}

		private void OnEnable()
		{
			if (!Application.isPlaying && activeInstance == null)
			{
				activeInstance = this;
			}
		}

		private void OnDisable()
		{
			if (activeInstance == this)
			{
				activeInstance = null;
			}
		}

		private void OnDestroy()
		{
			if (activeInstance == this)
			{
				activeInstance = null;
			}
		}

		private static void EditorUpdate()
		{
			if (Application.isPlaying || activeInstance == null || activeInstance.disableMe) return;

			var eg = activeInstance;
			var s = Selection.activeTransform;
			if (s == null) return;
			if (s == eg.prevSelect || s == eg.prevSelectWrong) return;

			if (s == eg.transform)
			{
				if (eg.selfSelected) return;
				eg.selfSelected = true;
				eg.DisableAllChildren();
				return;
			}

			if (s.parent == null || s.name.Contains(eg.ignoreTag) || s.parent != eg.transform) return;

			if (eg.prevSelect != null)
			{
				eg.selfSelected = false;
				eg.prevSelect.gameObject.SetActive(false);
				s.gameObject.SetActive(true);

				var pss = s.GetComponent<ParticleSystem>();
				if (pss)
				{
					pss.time = 0;
					pss.Stop();
				}
			}

			eg.prevSelect = s;
			eg.CountParticles();
			eg.PlayParticles();
		}
#endif

		void Start()
		{
			if (!Application.isPlaying) return;
			if (disableOnPlay)
			{
				gameObject.SetActive(false);
				return;
			}

			if (randomEnable)
			{
				DisableAllChildren();
				randomEnablePlaceHolder = new GameObject("randomEnablePlaceHolder").transform;
			}

			if (autoSlideShow)
			{
				DisableAllChildren();
			}
		}

		void Update()
		{
			if (!Application.isPlaying) return;
			if (autoSlideShowStarted) return;

			if (Input.GetKeyUp(nextKey))
			{
				if (autoSlideShow)
				{
					if (randomEnable)
						InvokeRepeating(nameof(RandomModel), 0, autoSlideShowDelay);
					else
						InvokeRepeating(nameof(NextModel), 0, autoSlideShowDelay);

					autoSlideShowStarted = true;
				} else
				{
					if (randomEnable)
						RandomModel();
					else
						NextModel();
				}
			}
		}

		void RandomModel()
		{
			if (transform.childCount == 0 && !repeat) return;

			if (transform.childCount == 0 && repeat)
			{
				while (randomEnablePlaceHolder.childCount > 0)
				{
					randomEnablePlaceHolder.GetChild(0).parent = transform;
				}
			}

			if (prevRandomBird != null)
			{
				prevRandomBird.SetActive(false);
				prevRandomBird.transform.SetParent(randomEnablePlaceHolder.transform);
			}

			if (transform.childCount == 0) return;

			int randomBird = Random.Range(0, transform.childCount);
			prevRandomBird = transform.GetChild(randomBird).gameObject;
			prevRandomBird.SetActive(true);
		}

		void PlayParticles()
		{
			if (!prevSelect) return;

			ParticleSystem pss = prevSelect.GetComponent<ParticleSystem>();
			if (!pss) return;

			pss.Clear();
			pss.Play();
			pss.Emit(emitOnEnable);
		}

		void DisableAllChildren()
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform t = transform.GetChild(i);
				if (t.gameObject.activeInHierarchy && t.parent == transform)
				{
					t.gameObject.SetActive(false);
				}
			}
		}

		void NextModel()
		{
			if (transform.childCount == 0) return;

			DisableAllChildren();
			transform.GetChild(counter % transform.childCount).gameObject.SetActive(true);
			counter++;
		}

		void CountParticles()
		{
			ParticleSystem[] obj = transform.GetComponentsInChildren<ParticleSystem>(true);
			particleSystems = 0;

			for (int i = 0; i < obj.Length; i++)
			{
				if (obj[i].transform.parent != null)
				{
					if (obj[i].transform.parent.GetComponent<ParticleSystem>() == null)
					{
						particleSystems++;
					}
				}
			}
		}
	}
}
