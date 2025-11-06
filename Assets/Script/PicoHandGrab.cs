using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using XRoam.Experience.Colliders;

public class PicoHandGrab : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] Vector3 snapPositionRight;
    [SerializeField] Vector3 snapPositionLeft;
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

    public void ResetPosition(Transform interactor)
    {
        
        if (interactor.TryGetComponent<UserRigInteractor>(out UserRigInteractor userRigInteractor))
        {
            if (userRigInteractor.BodyPartType == RigBodyPartType.LeftHand)
            {
                transform.localPosition = snapPositionLeft;
                transform.localRotation = Quaternion.Euler(-180f, 0f, 270f);
            }
            else if (userRigInteractor.BodyPartType == RigBodyPartType.RightHand)
            {
                transform.localPosition = snapPositionRight;
                transform.localRotation = Quaternion.Euler(-180f, 0f, 90f);
            }
        }

        
    }
}
