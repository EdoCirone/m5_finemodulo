using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDetector : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _viewAngle = 45f;
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] Transform _eyePosition;
    [SerializeField] LayerMask _obstacleMask;

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
}