using UnityEngine;
using System.Collections;

public class PortalDelayedAnimation : MonoBehaviour
{
    public Animator animator;
    public string stateName;
    public float delay;

    private bool started = false;

    private void OnTriggerEnter(Collider other)
    {
        if (started) return;

        started = true;
        StartCoroutine(PlayAfterDelay());
    }

    IEnumerator PlayAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        animator.Play(stateName);
    }
}