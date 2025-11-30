using UnityEngine;

public class PlayAnimatorAfterDelay : MonoBehaviour
{
    public Animator animator;
    public string triggerName;
    public float delay;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        activated = true;
        StartCoroutine(ActivateAfterDelay());
    }

    private System.Collections.IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(triggerName);
    }
}