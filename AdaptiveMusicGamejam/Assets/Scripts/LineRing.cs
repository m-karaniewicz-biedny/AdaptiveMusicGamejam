using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRing : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int pointCount = 3;
    [SerializeField] private int distance = 3;

    private void OnValidate()
    {
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPositions(GetPositions(distance, pointCount));
    }

    private Vector3[] GetPositions(float distance, int pointCount)
    {
        Vector3[] points = new Vector3[pointCount];
        Vector3 offset = new Vector3(0, 0, distance);

        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * 360 / pointCount;

            Vector3 pos = transform.position + Quaternion.AngleAxis(angle, Vector3.up) * offset;

            points[i] = pos;
        }

        return points;
    }
}
