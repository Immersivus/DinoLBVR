using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMovement : MonoBehaviour
{

    [SerializeField] SplineContainer splineContainer;
    public float speed = 2f;
    public float rotationMultiplier = 2f;

    [Header("Stops")]
    public List<SplineStop> stops;

    private float t = 0f;
    private bool isWaiting = false;
    private int currentStopIndex = 0;

    [Header("Movement")]
    public float maxSpeed = 6f;
    public float acceleration = 4f;
    public float deceleration = 6f;

    private float currentSpeed = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        splineContainer = GameObject.FindGameObjectWithTag("Spline").GetComponent<SplineContainer>();
        // Snap to spline start
        transform.position = splineContainer.EvaluatePosition(0f);

        // If first stop is knot 0
        if (stops.Count > 0 && stops[0].knotIndex == 0)
        {
            yield return StartCoroutine(
                Wait(stops[0].waitTime)
            );

            currentStopIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (splineContainer == null || isWaiting) return;

        int targetKnot = stops[currentStopIndex].knotIndex;

        float distanceToStop = DistanceToKnot(targetKnot);

        // Calculate required stopping distance
        float stoppingDistance =
            (currentSpeed * currentSpeed) / deceleration;

        // Decide whether to accelerate or brake
        if (distanceToStop <= stoppingDistance)
        {
            // BRAKE
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                deceleration * Time.deltaTime
            );
        }
        else
        {
            // ACCELERATE
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                maxSpeed,
                acceleration * Time.deltaTime
            );
        }

        float splineLength = splineContainer.CalculateLength();

        t += (currentSpeed / splineLength) * Time.deltaTime;

        transform.position = splineContainer.EvaluatePosition(t);

        CheckStops(distanceToStop);

        Vector3 direction = splineContainer.EvaluateTangent(t);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationMultiplier
            );
        }

        Debug.Log(distanceToStop);
    }

    void CheckStops(float distanceToStop)
    {
        if (distanceToStop < 0.05f && currentSpeed < 0.05f)
        {
            int knotIndex = stops[currentStopIndex].knotIndex;

            Vector3 exactPos = splineContainer.transform.TransformPoint(
                splineContainer.Spline[knotIndex].Position
            );

            transform.position = exactPos;

            StartCoroutine(
                Wait(stops[currentStopIndex].waitTime)
            );

            currentStopIndex++;

            maxSpeed = stops[currentStopIndex].speedToKnot;
            acceleration = maxSpeed - 0.2f;
            deceleration = maxSpeed;
        }
    }

    IEnumerator Wait(float seconds)
    {
        isWaiting = true;
        
        yield return new WaitForSeconds(seconds);
        isWaiting = false;
    }

    float DistanceToKnot(int knotIndex)
    {
        Vector3 knotPos = splineContainer.transform.TransformPoint(
            splineContainer.Spline[knotIndex].Position
        );

        return Vector3.Distance(transform.position, knotPos);
    }
}

[System.Serializable]

public class SplineStop
{
    public int knotIndex;
    public float waitTime;
    public float speedToKnot;
}
