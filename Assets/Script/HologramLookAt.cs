using UnityEngine;
using System.Collections.Generic;
public class HologramLookAt : MonoBehaviour
{

    Transform playerHead;

    [SerializeField] PicoHandGrab pickaxe;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnAwake()
    {
        pickaxe.DisableOnHologram();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHead == null)
        {
            if (Camera.main != null)
                playerHead = Camera.main.transform;
            else
                return; // just skip this frame, no errors
        }

        Vector3 lookDir = playerHead.position - transform.position;

        lookDir.y = 0f;

        // If the player is directly above/below, ignore to avoid NaN
        if (lookDir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
