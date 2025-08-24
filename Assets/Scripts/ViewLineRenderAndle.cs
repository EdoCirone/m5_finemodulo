using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewLineRenderAndle : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] private float _circleRadius = 5f;
    [SerializeField] private int _subdivisions = 12;
    void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        EvalutateCircle(_subdivisions);
    }

    public void EvalutateCircle(int subdivisions)
    {
        _lineRenderer.positionCount = subdivisions;

        Vector3[] positions = new Vector3[subdivisions];

        float deltaAngle = Mathf.PI * 2 / subdivisions;

        for (int i = 0; i < subdivisions; i++)
        {
            float currentAngle = deltaAngle * i;
            positions[i].x = Mathf.Cos(currentAngle) * _circleRadius;
            positions[i].z = Mathf.Sin(currentAngle) * _circleRadius;
        }

        _lineRenderer.SetPositions(positions);
    }

}
