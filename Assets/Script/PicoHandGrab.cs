using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class PicoHandGrab : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] Vector3 snapPosition;
    [SerializeField] Quaternion startingRotation;

    private void Start()
    {
        startingPosition = gameObject.transform.localPosition;
        startingRotation = gameObject.transform.localRotation;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Check"))
        {
            transform.SetParent(parent.transform);
            transform.localPosition = startingPosition;
            transform.localRotation = startingRotation;
        }
    }

    public void ResetPosition()
    {
        transform.localPosition = snapPosition;
        transform.localRotation = Quaternion.Euler(-180f, 0f, 90f);
    }
}
