using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float _speedSmoothTime = 0.1f;

    private Animator _animator;
    private PlayerControl _playerControl;

    private float _speedVelocity;

    // Hash dell'ID del parametro per performance migliori
    private int _speedHash;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerControl = GetComponent<PlayerControl>();

        // Pre-calcola l'hash del parametro
        _speedHash = Animator.StringToHash("Speed");
    }

    void Update()
    {
        if (_animator == null || _playerControl == null) return;

        UpdateAnimationParameters();
    }

    void UpdateAnimationParameters()
    {

        float currentSpeed = _playerControl.CurrentSpeed;

        float smoothedSpeed = Mathf.SmoothDamp(
            _animator.GetFloat(_speedHash),
            currentSpeed,
            ref _speedVelocity,
            _speedSmoothTime
        );

        _animator.SetFloat(_speedHash, smoothedSpeed);
    }

    // Metodo per controllare la velocità generale delle animazioni
    public void SetAnimationSpeed(float speed)
    {
        if (_animator != null)
            _animator.speed = speed;
    }
}
