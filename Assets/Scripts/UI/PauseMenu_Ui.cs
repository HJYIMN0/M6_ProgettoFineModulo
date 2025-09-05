using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu_Ui : Singleton<PauseMenu_Ui>
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    [Header("Singleton Settings")]
    [SerializeField] private string[] _scenesToSkipDontDestroy = { "Main Menu", "MainMenu", "Menu" };

    private CanvasGroup _canvasGroup;
    private bool _isPaused;

    protected override void OnSingletonAwake()
    {
        // Controlla se la scena corrente è in quelle da escludere
        string currentSceneName = SceneManager.GetActiveScene().name;
        bool shouldSkipDontDestroy = false;

        foreach (string sceneToSkip in _scenesToSkipDontDestroy)
        {
            if (currentSceneName == sceneToSkip)
            {
                shouldSkipDontDestroy = true;
                break;
            }
        }

        // Applica DontDestroyOnLoad solo se non siamo in una scena da escludere
        if (!shouldSkipDontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }

        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (Instance == this) // Esegui solo se questa è l'istanza valida
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0f;
            _isPaused = false;
        }
    }

    private void Update()
    {
        if (Instance == this && Input.GetKeyDown(KeyCode.Escape))
        {
            _canvasGroup.interactable = true;
            CallPauseMenu();
        }
    }

    public void CallPauseMenu()
    {
        if (!_isPaused) Pause();
        else if (_isPaused) Resume();
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
    }

    public void Quit()
    {
        // Assicurati di ripristinare il timeScale prima di cambiare scena
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartScene()
    {
        // Ripristina il timeScale prima di riavviare la scena
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void TestButtons() => Debug.Log("Button pressed!");

    // Metodo per aggiornare dinamicamente le scene da escludere
    public void SetScenestoSkipDontDestroy(string[] newScenes)
    {
        _scenesToSkipDontDestroy = newScenes;
    }

    // Metodo per controllare se il menu è attualmente in pausa
    public bool IsPaused => _isPaused;
}