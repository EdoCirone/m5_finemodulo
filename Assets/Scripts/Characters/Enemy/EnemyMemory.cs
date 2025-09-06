using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMemory : MonoBehaviour
{
    public Vector3? LastKnownPlayerPosition { get; set; }
    public Vector3? FirstSightPosition { get; set; }
    public Quaternion? FirstSightRotation { get; set; }

    public Vector3? EnemyPositionAtFirstSight { get; set; }     
    public Quaternion? EnemyRotationAtFirstSight { get; set; }   

    public float TimeLostPlayer { get; set; }

    public bool HasLastKnownPosition => LastKnownPlayerPosition.HasValue;

    public void ClearMemory()
    {
        LastKnownPlayerPosition = null;
        FirstSightPosition = null;
        FirstSightRotation = null;
        EnemyPositionAtFirstSight = null;
        EnemyRotationAtFirstSight = null;
        TimeLostPlayer = 0f;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Primo avvistamento
        if (FirstSightPosition.HasValue)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(FirstSightPosition.Value, 0.25f);
            Gizmos.DrawLine(transform.position, FirstSightPosition.Value);
        }

        // Ultima posizione nota
        if (LastKnownPlayerPosition.HasValue)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(LastKnownPlayerPosition.Value, 0.2f);
            Gizmos.DrawLine(transform.position, LastKnownPlayerPosition.Value);
        }

        // Posizione del nemico al primo avvistamento
        if (EnemyPositionAtFirstSight.HasValue)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(EnemyPositionAtFirstSight.Value, Vector3.one * 0.2f);
            Gizmos.DrawLine(transform.position, EnemyPositionAtFirstSight.Value);
        }
    }
}