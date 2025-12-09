using UnityEngine;


[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    public CanvasGroup iconGroup;    // CanvasGroup on the icon
    public float fadeSpeed = 5f;     // Speed of fade
    public float maxDistance = 5f;   // How close the player must be

    public float gazeRadius = 0.3f; // <-- make this bigger to make detection easier

    Transform playerHead;
    bool shouldShow = false;

    [SerializeField] Animator anim;

    void Start()
    {
        if (iconGroup != null)
            iconGroup.alpha = 0f;
    }

    void FixedUpdate()
    {
        /*if (playerHead == null)
        {
            if (Camera.main != null)
                playerHead = Camera.main.transform;
            else
                return;
        }

        // Get everything inside the gaze radius around the camera
        Collider[] hits = Physics.OverlapSphere(playerHead.position, gazeRadius);

        bool isHit = false;

        // Check if THIS object was inside the sphere
        foreach (var col in hits)
        {
            if (col.gameObject == gameObject)
            {
                isHit = true;
                break;
            }
        }

        shouldShow = isHit;

        // Smooth fade
        float targetAlpha = shouldShow ? 1f : 0f;
        iconGroup.alpha = Mathf.Lerp(iconGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

        // Billboard look-at (Y axis only)
        Vector3 lookDir = playerHead.position - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.001f)
            iconGroup.transform.rotation = Quaternion.LookRotation(lookDir);*/
    }

    public void PlayAnimation()
    {
        if(anim != null)
        {
            anim.SetTrigger("TRIGGER");
        }
    }

    
}

