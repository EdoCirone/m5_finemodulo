using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour
{
    public static SceneFlow Instance { get; private set; }

    [Header("Config")]
    [SerializeField] private string _mainMenuScene = "MainMenu";
    [SerializeField] private bool _restartOnAnyHit = true;
    [SerializeField] private float _restartDelay = 0.2f; 
    [SerializeField] private float _menuDelay = 0.2f;

    private bool _isLoading;
    private Coroutine _loadingCo; // traccia l’operazione in corso

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        var plc = FindObjectOfType<PlayerLifeControl>();
        if (plc != null) RegisterPlayer(plc);
    }

    public void RegisterPlayer(PlayerLifeControl plc)
    {
        plc.Damaged -= OnPlayerDamaged;
        plc.Died -= OnPlayerDied;

        plc.Damaged += OnPlayerDamaged;
        plc.Died += OnPlayerDied;
    }

    private void OnPlayerDamaged(int current, int max)
    {
        if (!_restartOnAnyHit) return;
        if (_isLoading || _loadingCo != null) return;

        _loadingCo = StartCoroutine(RestartLevelAfterDelay(_restartDelay));
    }

    private void OnPlayerDied()
    {
        // Priorità alla morte: cancella eventuale restart già schedulato
        if (_loadingCo != null)
        {
            StopCoroutine(_loadingCo);
            _loadingCo = null;
            _isLoading = false;
        }

        // Reset pv per la prossima scena
        PlayerLifeControl.ResetHealthToMax();

        if (SceneExistsInBuild(_mainMenuScene))
            _loadingCo = StartCoroutine(LoadSceneAfterDelay(_mainMenuScene, _menuDelay));
        else
            _loadingCo = StartCoroutine(RestartLevelAfterDelay(_menuDelay));
    }

    public void LevelCompleted()
    {
        if (_isLoading || _loadingCo != null) return;

        if (SceneExistsInBuild(_mainMenuScene))
            _loadingCo = StartCoroutine(LoadSceneAfterDelay(_mainMenuScene, _menuDelay));
        else
            _loadingCo = StartCoroutine(LoadNextBuildIndex());
    }

    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        _isLoading = true;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _isLoading = false;
        _loadingCo = null;
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        _isLoading = true;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
        _isLoading = false;
        _loadingCo = null;
    }

    private IEnumerator LoadNextBuildIndex()
    {
        _isLoading = true;
        int idx = SceneManager.GetActiveScene().buildIndex;
        int next = (idx + 1) % SceneManager.sceneCountInBuildSettings;
        yield return null;
        SceneManager.LoadScene(next);
        _isLoading = false;
        _loadingCo = null;
    }

    private bool SceneExistsInBuild(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName) return true;
        }
        return false;
    }
}
