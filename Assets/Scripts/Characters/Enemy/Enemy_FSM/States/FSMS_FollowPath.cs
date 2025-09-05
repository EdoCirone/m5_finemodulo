using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FSMS_FollowPath : AbstractFSMState
{
    private const float DestinationDistanceError = 0.01f;

    [SerializeField] private Transform[] _destinations;
    [SerializeField] private bool _walkingInLoop;

    private int _destinationIndex = 0;
    private bool _movingForward = true;
    private NavMeshAgent _agent;

    public override void StateEnter()
    {
        if (_agent == null)
            _agent = GetComponentInParent<NavMeshAgent>();

        if (_destinations.Length == 0)
        {
            Debug.LogWarning("Nessuna destinazione assegnata.");
            return;
        }

        _destinationIndex = GetClosestDestinationIndex();

        _agent.isStopped = false;
        SetDestinationAtIndex(_destinationIndex);
    }

    public override void StateUpdate()
    {
        if (_destinations.Length == 0 || _agent == null) return;

        if (IsCloseEnoughToDestination())
        {
            AdvancePath();
        }
    }

    public override void StateExit()
    {
      _agent.isStopped = true;  
    }

    public override void StateFixedUpdate() { }

    private int GetClosestDestinationIndex()
    {
        int closest = 0;
        float minDist = Vector3.SqrMagnitude(_destinations[0].position - transform.position);

        for (int i = 1; i < _destinations.Length; i++)
        {
            float dist = Vector3.SqrMagnitude(_destinations[i].position - transform.position);
            if (dist < minDist)
            {
                closest = i;
                minDist = dist;
            }
        }

        return closest;
    }

    private void AdvancePath()
    {
        if (_walkingInLoop)
        {
            SetDestinationAtIndex(_destinationIndex + 1);
        }
        else
        {
            PingPongMovement();
        }
    }

    private void PingPongMovement()
    {
        if (_movingForward)
        {
            if (_destinationIndex + 1 >= _destinations.Length)
            {
                _movingForward = false;
                SetDestinationAtPreviousIndex();
            }
            else
            {
                SetDestinationAtNextIndex();
            }
        }
        else
        {
            if (_destinationIndex - 1 < 0)
            {
                _movingForward = true;
                SetDestinationAtNextIndex();
            }
            else
            {
                SetDestinationAtPreviousIndex();
            }
        }
    }

    private void SetDestinationAtIndex(int index)
    {
        if (_destinations.Length == 0) return;

        if (index < 0) index += _destinations.Length;
        else if (index >= _destinations.Length) index %= _destinations.Length;

        _destinationIndex = index;
        _agent.SetDestination(_destinations[_destinationIndex].position);
    }

    private void SetDestinationAtNextIndex() => SetDestinationAtIndex(_destinationIndex + 1);
    private void SetDestinationAtPreviousIndex() => SetDestinationAtIndex(_destinationIndex - 1);

    private bool IsCloseEnoughToDestination()
    {
        return !_agent.pathPending &&
               _agent.remainingDistance <= _agent.stoppingDistance &&
               (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
    }
}
