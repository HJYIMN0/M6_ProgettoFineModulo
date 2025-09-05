using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu_Ui : MonoBehaviour
{    
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    private CanvasGroup _canvasGroup;
    private bool _isPaused;


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
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
            _canvasGroup.interactable = true;
            CallPauseMenu();
        }
    }
    public void CallPauseMenu()
    {
        if (!_isPaused)         Pause();
        else if (_isPaused)     Resume();
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
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void TestButtons() => Debug.Log("Button pressed!");
}

