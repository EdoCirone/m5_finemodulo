using System.Collections;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _lives;
    [SerializeField] private PlayerLifeControl _lifeControl;

    private void OnEnable()
    {
        if (_lifeControl == null) _lifeControl = FindObjectOfType<PlayerLifeControl>();
        if (_lifeControl == null) return;

        _lifeControl.Damaged += OnDamaged;
        _lifeControl.Died += OnDied;
    }

    private void Start()
    {
        //Aspetta un frame: PlayerLifeControl inizializza prima di LifeUI
        StartCoroutine(InitHeartsNextFrame());
    }

    private void OnDisable()
    {
        if (_lifeControl == null) return;
        _lifeControl.Damaged -= OnDamaged;
        _lifeControl.Died -= OnDied;
    }

    private IEnumerator InitHeartsNextFrame()
    {
        yield return null; // aspetta 1 frame
        int current = PlayerLifeControl.StaticCurrentHealth >= 0
            ? PlayerLifeControl.StaticCurrentHealth
            : _lifeControl.MaxHealth;

        UpdateHearts(current, _lifeControl.MaxHealth);
    }

    private void OnDamaged(int current, int max) => UpdateHearts(current, max);
    private void OnDied() => UpdateHearts(0, _lifeControl.MaxHealth);

    private void UpdateHearts(int current, int max)
    {
        for (int i = 0; i < _lives.Length; i++)
            _lives[i].SetActive(i < current);
    }
}
