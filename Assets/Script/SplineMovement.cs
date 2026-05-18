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

    public float rotationAcceleration = 180f;
    public float rotationDeceleration = 360f;

    private float currentAngularSpeed = 0f;

    private Quaternion targetRotation;
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

        RotateTowardsNextKnot(stops[currentStopIndex]);
    }

    void RotateTowardsNextKnot(SplineStop currentStop)
    {
        int nextKnotIndex =
            Mathf.Min(
                currentStop.knotIndex,
                splineContainer.Spline.Count - 1
            );

        Vector3 nextKnotPos =
            splineContainer.transform.TransformPoint(
                splineContainer.Spline[nextKnotIndex].Position
            );

        Vector3 direction =
            (nextKnotPos - transform.position).normalized;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        targetRotation =
            Quaternion.LookRotation(direction);

        float angleRemaining =
            Quaternion.Angle(
                transform.rotation,
                targetRotation
            );

        if (angleRemaining < 0.1f)
        {
            transform.rotation = targetRotation;
            return;
        }

        float stoppingAngle =
            (currentAngularSpeed * currentAngularSpeed) /
            (2f * rotationDeceleration);

        // Accelerate or decelerate
        if (angleRemaining <= stoppingAngle)
        {
            currentAngularSpeed = Mathf.MoveTowards(
                currentAngularSpeed,
                0f,
                rotationDeceleration * Time.deltaTime
            );
        }
        else
        {
            currentAngularSpeed = Mathf.MoveTowards(
                currentAngularSpeed,
                currentStop.maxRotationSpeed,
                rotationAcceleration * Time.deltaTime
            );
        }

        float rotationStep =
            Mathf.Min(
                currentAngularSpeed * Time.deltaTime,
                angleRemaining
            );

        transform.rotation =
            Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationStep
            );
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
    public float maxRotationSpeed = 10f;
}
