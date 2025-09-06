using UnityEngine;
using UnityEngine.AI;

public class FSMS_ReturnToOrigin : AbstractFSMState
{
    [SerializeField] private float _rotationSpeed = 180f;

    private NavMeshAgent _agent;
    private EnemyMemory _memory;

    private bool _isRotate = false; //Mi serve per ruotare e poi muovermi

    public override void Setup(FSMController controller)
    {
        base.Setup(controller);
        _agent = controller.GetComponentInParent<NavMeshAgent>();
        _memory = controller.GetComponentInParent<EnemyMemory>();
    }

    public override void StateEnter()
    {
        _isRotate = false;

       
        _agent.isStopped = true;
        _agent.ResetPath();
        _agent.updateRotation = false; // disabilito la rotazione del navmesh
        Debug.Log("[FSMS_ReturnToOrigin] Entrato nello stato. Inizio rotazione.");
    }

    public override void StateUpdate()
    {
        if (_memory == null || !_memory.EnemyPositionAtFirstSight.HasValue)
            return;

        if (!_isRotate)
        {
            Vector3 toDestination = _memory.EnemyPositionAtFirstSight.Value - _agent.transform.position;
            toDestination.y = 0f;

            if (toDestination.sqrMagnitude < 0.01f)
            {
                Debug.Log("Direzione troppo corta, ritorno.");
                return;
            }

            Quaternion desiredRotation = Quaternion.LookRotation(toDestination);
            _agent.transform.rotation = Quaternion.RotateTowards(
                _agent.transform.rotation,
                desiredRotation,
                _rotationSpeed * Time.deltaTime
            );
            float angle = Quaternion.Angle(_agent.transform.rotation, desiredRotation);
            Debug.Log($"Angolo restante: {angle}");

            if (angle < 1f)
            {
                _isRotate = true;
                _agent.SetDestination(_memory.EnemyPositionAtFirstSight.Value);
                _agent.isStopped = false;

                Debug.Log("[FSMS_ReturnToOrigin] Rotazione completata. Inizio movimento.");
            }
        }
    }

    public override void StateExit()
    {
        if (_agent != null)
        {
            _agent.updateRotation = true; //Ripristina la rotazione del navMEsh
        }
    }

    public override void StateFixedUpdate() { }
}
