using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterDetector : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _viewAngle = 45f;
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] Transform _eyePosition;
    [SerializeField] LayerMask _obstacleMask;

    public float ViewAngle => _viewAngle;
    public float ViewDistance => _viewDistance;

    public LayerMask ObstacleMask => _obstacleMask;

    public Transform EyePosition => _eyePosition;
    public Transform Target => _target;


    private void Update()
    {
        if (CanSeeTarget())
        {
            Debug.Log("Target in sight!");
        }
        else
        {
            Debug.Log("Target not in sight!");
        }

    }

    public bool CanSeeTarget()
    {
        if (_target == null || _eyePosition == null) return false;

        Vector3 eye = _eyePosition.position;
        Vector3 tgt = _target.position;

        Vector3 toTarget = tgt - eye;

        float sqrdistanceToTarget = toTarget.sqrMagnitude;

        if (sqrdistanceToTarget > _viewDistance * _viewDistance)
        {
            return false;
        }

        float distanceToTarget = Mathf.Sqrt(sqrdistanceToTarget);
        toTarget /= distanceToTarget;

        if (Vector3.Dot(_eyePosition.forward, toTarget) < Mathf.Cos(_viewAngle * Mathf.Deg2Rad))
        {
            return false;
        }


        if (Physics.Raycast(_eyePosition.position, toTarget + Vector3.up * 0.01f, distanceToTarget, _obstacleMask))
        {
            Debug.Log("Obstacle in the way!");
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (_eyePosition == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_eyePosition.position, _eyePosition.forward * _viewDistance);

        Vector3 leftLimit = Quaternion.Euler(0, -_viewAngle, 0) * _eyePosition.forward;
        Vector3 rightLimit = Quaternion.Euler(0, _viewAngle, 0) * _eyePosition.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_eyePosition.position, leftLimit * _viewDistance);
        Gizmos.DrawRay(_eyePosition.position, rightLimit * _viewDistance);
    }
}