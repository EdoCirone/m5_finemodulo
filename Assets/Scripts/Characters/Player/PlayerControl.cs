using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private bool _mouseControl = true;

    [SerializeField] private float _dubleClickTime = 0.25f;
    [SerializeField] private float _speedMultiplier = 2f;



    private NavMeshAgent _agent;
    private Camera _mainCamera;

    private float _h;
    private float _v;

    private float _lastClickTime = -1f;
    private float _baseSpeed;
    private bool _isRunning = false;


    void Awake()
    {
        _mainCamera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(transform.position);
        _baseSpeed = _agent.speed;

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

    }


    public void UseWASDInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _agent.speed = _baseSpeed * _speedMultiplier;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ResetSpeed();
        }

        Vector3 direction = new Vector3(_h, 0, _v).normalized;

        _agent.velocity = direction * _agent.speed;
    }

    public void UseMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            float deltaTime = Time.time - _lastClickTime; 
            _lastClickTime = Time.time;

            if (deltaTime < _dubleClickTime)
            {
                Debug.Log("Double Clicked" + _lastClickTime);
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
