using UnityEngine;

public class RandomUpDown : MonoBehaviour
{
    public float speed = 2f;       // How fast the object moves
    public float height = 0.5f;    // Max height deviation (+/- from starting point)

    private float randomOffset;    // Random phase offset
    private Vector3 startPos;      // Original position of the object

    void Start()
    {
        // Store the starting position
        startPos = transform.position;

        // Randomize the sine wave offset for unique movement per instance
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // Calculate vertical offset using sine wave and apply it relative to the startPos
        float newY = startPos.y + Mathf.Sin(Time.time * speed + randomOffset) * height;

        // Update the object's position with the animated Y value
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
