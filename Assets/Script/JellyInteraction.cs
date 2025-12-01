using UnityEngine;

public class JellyInteraction : MonoBehaviour
{
    public float floatAmplitude = 0.5f;   // Up/down amplitude
    public float floatFrequency = 1f;     // Speed of bobbing
    public float returnSpeed = 2f;        // How fast it returns to start
    public float minVelocityForReturn = 0.05f;
    public float returnAcceleration = 0.5f;

    public float hitForceMultiplier = 5f;

    private Vector3 startPos;
    private Vector3 idleBasePos;
    private Rigidbody rb;

    private enum State { Idle, Knocked, Returning }
    private State currentState = State.Idle;

    private float floatOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.1f;
        startPos = transform.position;
        idleBasePos = startPos;
        // Random offset so multiple jellyfish don't sync
        floatOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdleFloat();
                break;

            case State.Knocked:
                if (rb.linearVelocity.magnitude < minVelocityForReturn)
                {
                    currentState = State.Returning;
                }
                break;

            case State.Returning:
                ReturnToStart();
                break;
        }
    }

    // -------------------------
    //  STATE BEHAVIORS
    // -------------------------

    void HandleIdleFloat()
    {
        float yOffset = Mathf.Sin((Time.time + floatOffset) * floatFrequency) * floatAmplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }

    void ReturnToStart()
    {
        Vector3 toHome = startPos - transform.position;

        // Apply gentle acceleration toward home
        rb.AddForce(toHome.normalized * returnAcceleration, ForceMode.Acceleration);

        // Close enough to stop drifting
        if (toHome.magnitude < 0.05f)
        {
            // Lock into position
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Update idle base to wherever we ended
            idleBasePos = transform.position;

            currentState = State.Idle;
        }
    }

    // -------------------------
    //  PUBLIC / HIT HANDLING
    // -------------------------
    public void Hit(Vector3 hitDirection)
    {
        currentState = State.Knocked;

        rb.AddForce(hitDirection * hitForceMultiplier, ForceMode.Impulse);
    }

    public void OnTouched(Transform collision)
    {
        Vector3 dir = (transform.position - collision.position).normalized;
        Hit(dir);
    }
}
