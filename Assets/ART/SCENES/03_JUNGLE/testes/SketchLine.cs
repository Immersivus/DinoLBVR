using UnityEngine;
using Shapes;
using System.Collections.Generic;
[RequireComponent(typeof(Polyline))]
public class SketchLine : MonoBehaviour
{
    public int pointCount = 4;
    public float length = 5f;
    [Header("Sketch Settings")]
    public float noiseFrequency = 2f;
    public float noiseAmplitude = 0.1f;
    public float thickness = 0.05f;
    Polyline line;
    void Awake()
    {
        line = GetComponent<Polyline>();
        
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (float)(pointCount - 1);
            Vector3 pos = new Vector3(t * length, 0, 0);
            float n = Mathf.PerlinNoise(t * noiseFrequency, Time.time * 0.3f);
            pos.y += (n - 0.5f) * noiseAmplitude;
            // cria um ponto completo
            PolylinePoint p = new PolylinePoint(pos, Color.black, thickness);
            line.points.Add(p);
        }
    }
    void Update()
    {
        
    }
}