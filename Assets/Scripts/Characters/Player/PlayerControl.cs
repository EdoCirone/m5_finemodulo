using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private bool _mouseControl = true;
    [SerializeField] private float _dubleClickTime = 0.25f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private float _rotationSpeed = 5f; // Velocità di rotazione per WASD

    private NavMeshAgent _agent;
    private Camera _mainCamera;
    private float _h;
    private float _v;
    private float _lastClickTime = -1f;
    private float _baseSpeed;
    private bool _isRunning = false;
    private Vector3 _lastPosition;
    private float _calculatedSpeed;

    // Proprietà pubblica per PlayerAnimControl
    public float CurrentSpeed => _mouseControl ? _agent.velocity.magnitude : _calculatedSpeed;
    public NavMeshAgent Agent => _agent;

    void Awake()
    {
        _mainCamera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(transform.position);
        _baseSpeed = _agent.speed;

        // IMPORTANTE: Disabilita la rotazione automatica del NavMeshAgent
        _agent.updateRotation = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _mouseControl = !_mouseControl;
            _agent.ResetPath();
        }

        if (_mouseControl)
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
            UseMouseInput();

            // Per il mouse control, ruota verso la direzione di movimento
            _agent.RotateTowardsMovement(_rotationSpeed);

            if (_isRunning && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                ResetSpeed();
                _isRunning = false;
            }
        }
        else
        {
            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");
            UseWASDInput();
        }

        Vector3 displacement = transform.position - _lastPosition;
        _calculatedSpeed = displacement.magnitude / Time.deltaTime;
        _lastPosition = transform.position;
    }

    public void UseWASDInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _agent.speed = _baseSpeed * _speedMultiplier;
            _isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ResetSpeed();
            _isRunning = false;
        }

        Vector3 direction = new Vector3(_h, 0, _v).normalized;

        if (direction != Vector3.zero)
        {
            // Per WASD: ruota immediatamente verso la direzione di input
            Vector3 worldDirection = Camera.main.transform.TransformDirection(direction);
            worldDirection.y = 0;
            worldDirection.Normalize();

            // Rotazione immediata verso la direzione di input
            Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            // Movimento
            _agent.Move(worldDirection * _agent.speed * Time.deltaTime);
        }
    }

    public void UseMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float deltaTime = Time.time - _lastClickTime;
            _lastClickTime = Time.time;

            if (deltaTime < _dubleClickTime)
            {
                _agent.speed = _baseSpeed * _speedMultiplier;
                _isRunning = true;
            }
            else
            {
                ResetSpeed();
                _isRunning = false;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }
    }

    public void ResetSpeed()
    {
        _agent.speed = _baseSpeed;
    }
}