using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMovement : MonoBehaviour
{

    [SerializeField] SplineContainer splineContainer;
    public float speed = 2f;

    [Header("Stops")]
    public List<SplineStop> stops;

    private float t = 0f;
    private bool isWaiting = false;
    private int currentStopIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineContainer = GameObject.FindGameObjectWithTag("Spline").GetComponent<SplineContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (splineContainer == null || isWaiting) return;
        t += speed * Time.deltaTime / splineContainer.CalculateLength();

        t = Mathf.Clamp01(t);

        Vector3 position = splineContainer.EvaluatePosition(t);
        transform.position = position;

        CheckStops();
    }

    void CheckStops()
    {
        if (currentStopIndex >= stops.Count) return;

        Vector3 knotPos = GetKnotPosition(stops[currentStopIndex].knotIndex);

        float distance = Vector3.Distance(transform.position, knotPos);

        if (distance < 0.1f)
        {
            StartCoroutine(Wait(stops[currentStopIndex].waitTime));
            currentStopIndex++;
        }
    }

    IEnumerator Wait(float seconds)
    {
        isWaiting = true;
        yield return new WaitForSeconds(seconds);
        isWaiting = false;
    }

    Vector3 GetKnotPosition(int index)
    {
        return splineContainer.Spline[index].Position;
    }
}

[System.Serializable]

public class SplineStop
{
    public int knotIndex;
    public float waitTime;
}
