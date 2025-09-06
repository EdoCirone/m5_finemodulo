using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VisionConeLineRendererHandler : MonoBehaviour
{
    [SerializeField] private int _subdivisions = 12;

    private LineRenderer _lineRenderer;
    private CharacterDetector _characterDetector;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _characterDetector = GetComponentInParent<CharacterDetector>();
        EvaluateConeOfView(_subdivisions);
    }

    void Update()
    {
        EvaluateConeOfView(_subdivisions);
    }

    public void EvaluateConeOfView(int subdivisions)
    {
        if (_characterDetector == null) return;

        Vector3 origin = _characterDetector.EyePosition.position;
        origin.y = 0f; 

        float viewAngle = _characterDetector.ViewAngle;
        float viewDistance = _characterDetector.ViewDistance;
        float halfFOV = viewAngle * 0.5f;
        float deltaAngle = viewAngle / (subdivisions - 1);


        Vector3[] positions = new Vector3[subdivisions + 1];

        for (int i = 0; i < subdivisions; i++)
        {
            float angle = -halfFOV + deltaAngle * i;

            
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up) * _characterDetector.EyePosition.rotation;
            Vector3 dir = rot * Vector3.forward;
            dir.y = 0f; 

            Vector3 point = origin + dir * viewDistance;

            if (Physics.Raycast(origin, dir, out RaycastHit hit, viewDistance, _characterDetector.ObstacleMask))
            {
                point = hit.point;
            }

            point.y = origin.y; 
            positions[i] = point;
        }

        
        positions[subdivisions] = origin;

        _lineRenderer.useWorldSpace = true;
        _lineRenderer.loop = true; 

        _lineRenderer.SetPositions(positions);
    }

}
