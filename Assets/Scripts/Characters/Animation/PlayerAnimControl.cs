using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float _speedSmoothTime = 0.1f;
    [SerializeField] private float _directionSmoothTime = 0.1f;
    [SerializeField] private float _movementThreshold = 0.1f;

    private Animator _animator;
    private PlayerControl _playerControl;

    // Valori smoothed per transizioni fluide
    private float _speedVelocity;
    private float _horizontalVelocity;
    private float _verticalVelocity;

    // Hash degli ID dei parametri per performance migliori
    private int _speedHash;
    private int _isMovingHash;
    private int _isRunningHash;
    private int _horizontalHash;
    private int _verticalHash;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerControl = GetComponent<PlayerControl>();

        // Pre-calcola gli hash dei parametri
        _speedHash = Animator.StringToHash("Speed");
        _isMovingHash = Animator.StringToHash("IsMoving");
        _isRunningHash = Animator.StringToHash("IsRunning");
        _horizontalHash = Animator.StringToHash("Horizontal");
        _verticalHash = Animator.StringToHash("Vertical");
    }

    void Update()
    {
        if (_animator == null || _playerControl == null) return;

        UpdateAnimationParameters();
    }

    void UpdateAnimationParameters()
    {
        // Calcola la velocità attuale
        float currentSpeed = _playerControl.Agent.velocity.magnitude;

        // Smooth della velocità per transizioni fluide
        float smoothedSpeed = Mathf.SmoothDamp(
            _animator.GetFloat(_speedHash),
            currentSpeed,
            ref _speedVelocity,
            _speedSmoothTime
        );

        // Determina se il personaggio si sta muovendo
        bool isMoving = currentSpeed > _movementThreshold;

        // Imposta i parametri base
        _animator.SetFloat(_speedHash, smoothedSpeed);
        _animator.SetBool(_isMovingHash, isMoving);
        _animator.SetBool(_isRunningHash, _playerControl.IsRunning);

        // Gestione dei parametri direzionali
        UpdateDirectionalParameters();
    }

    void UpdateDirectionalParameters()
    {
        float targetHorizontal = 0f;
        float targetVertical = 0f;

        if (!_playerControl.IsMouseControl)
        {
            // Per il controllo WASD, usa gli input diretti
            targetHorizontal = _playerControl.HorizontalInput;
            targetVertical = _playerControl.VerticalInput;
        }
        else
        {
            // Per il controllo mouse, calcola la direzione basata sul movimento
            Vector3 velocity = _playerControl.Agent.velocity.normalized;
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            if (velocity.magnitude > _movementThreshold)
            {
                targetVertical = Vector3.Dot(velocity, forward);
                targetHorizontal = Vector3.Dot(velocity, right);
            }
        }

        // Smooth dei parametri direzionali
        float smoothedHorizontal = Mathf.SmoothDamp(
            _animator.GetFloat(_horizontalHash),
            targetHorizontal,
            ref _horizontalVelocity,
            _directionSmoothTime
        );

        float smoothedVertical = Mathf.SmoothDamp(
            _animator.GetFloat(_verticalHash),
            targetVertical,
            ref _verticalVelocity,
            _directionSmoothTime
        );

        _animator.SetFloat(_horizontalHash, smoothedHorizontal);
        _animator.SetFloat(_verticalHash, smoothedVertical);
    }

    // Metodo per controllare la velocità generale delle animazioni (opzionale)
    public void SetAnimationSpeed(float speed)
    {
        if (_animator != null)
            _animator.speed = speed;
    }

}