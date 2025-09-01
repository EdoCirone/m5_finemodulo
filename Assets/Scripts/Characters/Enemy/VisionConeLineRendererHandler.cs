using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeLineRendererHandler : MonoBehaviour
{
    [SerializeField] private int _subdivisions = 12;


    private LineRenderer _lineRenderer;
    private CharacterDetector _characterDetector;
    void Start()
    {

        _lineRenderer = GetComponent<LineRenderer>();
        _characterDetector = GetComponentInParent<CharacterDetector>();
        EvalutateConeOfView(_subdivisions);

    }

    void Update()
    {
        EvalutateConeOfView(_subdivisions);
    }

    public void EvalutateConeOfView(int subdivisions)
    {
        Vector3 origin = _characterDetector.transform.position;

        // Ottieni solo la rotazione Y dagli occhi
        float eyeYaw = _characterDetector.EyePosition.eulerAngles.y;
        Quaternion eyeYawRotation = Quaternion.Euler(0f, eyeYaw, 0f);

        float viewAngle = _characterDetector.ViewAngle;
        float viewDistance = _characterDetector.ViewDistance;
        float halfAngleRad = viewAngle * Mathf.Deg2Rad;
        float deltaAngle = (halfAngleRad * 2f) / subdivisions;

        int points = subdivisions + 1;
        _lineRenderer.positionCount = points;

        Vector3[] positions = new Vector3[points];
        Vector3 raycastOrigin = origin + Vector3.up * 0.5f; // un po' sollevato per evitare terreno

        for (int i = 0; i < subdivisions; i++)
        {
            float currentAngle = -halfAngleRad + i * deltaAngle;
            Vector3 localDir = new Vector3(Mathf.Sin(currentAngle), 0f, Mathf.Cos(currentAngle));
            Vector3 worldDir = eyeYawRotation * localDir;

            Vector3 point = origin + worldDir * viewDistance;

            if (Physics.Raycast(raycastOrigin, worldDir, out RaycastHit hit, viewDistance, _characterDetector.ObstacleMask))
            {
                point = hit.point;
            }

            positions[i] = point;
        }

        positions[subdivisions] = origin; // chiudi il cono
        _lineRenderer.SetPositions(positions);
    }


}
