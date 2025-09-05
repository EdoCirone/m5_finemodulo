using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMemory : MonoBehaviour
{
    public Vector3? LastKnownPlayerPosition { get; set; }
    public Vector3? FirstSightPosition { get; set; }
    public float TimeLostPlayer { get; set; }

    public bool HasLastKnownPosition => LastKnownPlayerPosition.HasValue;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.green;

        if (FirstSightPosition.HasValue)
        {
            Gizmos.DrawSphere(FirstSightPosition.Value, 0.25f);
            Gizmos.DrawLine(transform.position, FirstSightPosition.Value);
        }

        Gizmos.color = Color.yellow;

        if (LastKnownPlayerPosition.HasValue)
        {
            Gizmos.DrawSphere(LastKnownPlayerPosition.Value, 0.2f);
            Gizmos.DrawLine(transform.position, LastKnownPlayerPosition.Value);
        }
    }
}
