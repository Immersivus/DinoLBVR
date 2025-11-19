using UnityEngine;

public class JellyInteraction : MonoBehaviour
{
    public float pushForce;
    public float buoyancy;
    public float wobbleAmount;
    public float wobbleSpeed;

    private Rigidbody rb;
    private float wobbleTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * buoyancy, ForceMode.Force);

        if (wobbleTimer > 0f)
        {
            float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
            rb.AddForce(transform.up * wobble, ForceMode.Acceleration);
            wobbleTimer -= Time.deltaTime;
        }
    }

    public void OnTouched(Transform toucher)
    {
        Vector3 dir = (transform.position - toucher.position).normalized;
        rb.AddForce(dir * pushForce, ForceMode.Impulse);

        wobbleTimer = 1f;
    }
}