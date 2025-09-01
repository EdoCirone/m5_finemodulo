using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour, IAlarmed
{
    [SerializeField] private Transform _doorTransform;

    [SerializeField] private float _openOffsetY;
    [SerializeField] private float _openDuration = 0.4f;


    private Coroutine _opendoor;
    private Vector3 _closePos;
    private Collider _collider;
    private bool _playerInside = false;
    private bool _isOpen = false;

    private void Awake()
    {

        _collider = GetComponent<Collider>();

    }

    private void Start()
    {
        _closePos = _doorTransform.position;
    }

    void Update()
    {
        if (_playerInside)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_isOpen) CloseDoor();
                else OpenDoor();
            }

        }
       ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = false;
        }
    }

    private void OpenDoor()
    {
        Vector3 openPos = _closePos + new Vector3(0f, _openOffsetY, 0f);
        StartMove(openPos);
        _isOpen = true;
    }

    private void ColseDoor()
    {

        StartMove(_closePos);
        _isOpen = false;

    }


    private void StartMove(Vector3 targetPos)
    {
        if (_moveCo != null) StopCoroutine(_moveCo);
        _moveCo = StartCoroutine(MoveDoorRoutine(targetPos));
    }

    private IEnumerator MoveDoorRoutine(Vector3 targetPos)
    {
        Vector3 startPos = _doorTransform.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, _moveDuration);
            _doorTransform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        _doorTransform.position = targetPos;
        _moveCo = null;
    }

    // ---- IAlarmed ----
    public void OnAlarmRaised()
    {
        // chiudi la porta e disabilita l’interazione (il pulsante non funziona più)
        CloseDoor();
        if (_col != null) _col.enabled = false;
    }

    public void OnAlarmLowered()
    {
        // riabilita l’interazione (non apre automaticamente)
        if (_col != null) _col.enabled = true;
    }
}
