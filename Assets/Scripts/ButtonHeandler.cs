using System.Collections;
using TMPro;
using UnityEngine;

public class ButtonHandler : MonoBehaviour, IAlarmed
{
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private GameObject _uiPrompt;

    [SerializeField] private float _openOffsetY;
    [SerializeField] private float _openDuration = 0.4f;

    private Coroutine _moveCo;
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
        if (_playerInside && Input.GetKeyDown(KeyCode.F))
        {
            if (_isOpen) CloseDoor();
            else OpenDoor();
        }

        if (_playerInside && _uiPrompt != null)
        {

            TMP_Text tmpText = _uiPrompt.GetComponentInChildren<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = _isOpen ? "Premi [F] per chiudere" : "Premi [F] per aprire";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = true;
            if (_uiPrompt != null) _uiPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = false;
            if (_uiPrompt != null) _uiPrompt.SetActive(false);
        }
    }

    private void OpenDoor()
    {
        Vector3 openPos = _closePos + new Vector3(0f, _openOffsetY, 0f);
        StartMove(openPos);
        _isOpen = true;
    }

    private void CloseDoor()
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
            t += Time.deltaTime / Mathf.Max(0.0001f, _openDuration);
            _doorTransform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        _doorTransform.position = targetPos;
        _moveCo = null;
    }

    // ---- IAlarmed ----
    public void OnAlarmRaised()
    {
        CloseDoor();
        if (_collider != null) _collider.enabled = false;
    }

    public void OnAlarmLowered()
    {
        if (_collider != null) _collider.enabled = true;
    }
}
