using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource track01, track02;

    public static AudioManager instance;

    bool isPlayingTrack1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        isPlayingTrack1 = true;
    }

    public void SwapTrack(AudioSource newSource)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(newSource));
    }

    private IEnumerator FadeTrack(AudioSource newSource)
    {
        float timeToFade = 3f;
        float timeElapsed = 0;

        track01 = track02;
        track02 = newSource;

        track02.Play();

        while (timeElapsed < timeToFade)
        {
            track02.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            track01.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        track01.Stop();
    }
}
