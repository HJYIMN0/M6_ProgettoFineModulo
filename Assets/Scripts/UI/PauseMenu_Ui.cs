using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu_Ui : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private bool _isPaused;
    private TimeManager _timerInstance;
    public bool IsPaused => _isPaused;

    public void Awake()
    {       
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup not found! Remember to add the component or to check the component position!");
            return;
        }
        else
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0f;
            _isPaused = false;
        }
        _timerInstance = TimeManager.Instance;
    }

    private void Start()
    {        
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0f;
        _isPaused = false;        
    }   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _timerInstance.PauseGame();
    }

    public void Resume()
    {
        _isPaused = false;
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _timerInstance.ResumeGame();
    }

    public void Quit()
    {
        // Assicurati di ripristinare il timeScale prima di cambiare scena
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartScene()
    {
        // Ripristina il timeScale prima di riavviare la scena
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    //public void TestButtons() => Debug.Log("Button pressed!");

}