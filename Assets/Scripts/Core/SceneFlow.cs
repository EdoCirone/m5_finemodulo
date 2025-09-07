using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour
{
    public static SceneFlow Instance { get; private set; }

    [SerializeField] private string _mainMenuScene = "MainMenu";
    [SerializeField] private bool _restartOnAnyHit = true;
    [SerializeField] private float _delay = 0.2f;

    private bool _busy;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
     
    }
    public void RegisterPlayer(PlayerLifeControl lifeControl)
    {
        if (lifeControl == null) return;

        // pulizia/ri-subscribe idempotente
        lifeControl.Damaged -= OnPlayerDamaged;
        lifeControl.Died -= OnPlayerDied;

        lifeControl.Damaged += OnPlayerDamaged;
        lifeControl .Died += OnPlayerDied;
    }

    public void LevelCompleted()
    {
        if (_busy) return;
        LoadNextOrMenuAfter(_delay);
    }

    private void OnPlayerDamaged(int current, int max)
    {
        if (!_restartOnAnyHit || _busy) return;
        RestartCurrentAfter(_delay);
    }

    private void OnPlayerDied()
    {
        if (_busy) return;
        PlayerLifeControl.ResetHealthToMax();
        LoadMainMenuOrRestartAfter(_delay);
    }

    private void RestartCurrentAfter(float d)
    {
        StartCoroutine(WaitThen(d, () =>
        {
            _busy = true;
            Time.timeScale = 1f;
            var cur = SceneManager.GetActiveScene();
            SceneManager.LoadScene(cur.buildIndex);
            _busy = false;
        }));
    }

    private void LoadMainMenuOrRestartAfter(float d)
    {
        StartCoroutine(WaitThen(d, () =>
        {
            _busy = true;
            Time.timeScale = 1f;

            if (Application.CanStreamedLevelBeLoaded(_mainMenuScene))
                SceneManager.LoadScene(_mainMenuScene);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            _busy = false;
        }));
    }

    private void LoadNextOrMenuAfter(float d)
    {
        StartCoroutine(WaitThen(d, () =>
        {
            _busy = true;
            Time.timeScale = 1f;

            int cur = SceneManager.GetActiveScene().buildIndex;
            int next = cur + 1;

            if (next < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(next);
            else if (Application.CanStreamedLevelBeLoaded(_mainMenuScene))
                SceneManager.LoadScene(_mainMenuScene);
            else
                SceneManager.LoadScene(cur); // fallback

            _busy = false;
        }));
    }

    private IEnumerator WaitThen(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}
