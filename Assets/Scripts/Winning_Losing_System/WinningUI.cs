using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinningUI : MonoBehaviour
{    
    [SerializeField] private TextMeshProUGUI _timerText;

    private CanvasGroup _canvasGroup;
    private CanvasGroupFader _canvasGroupFader;
    private Timer _timer;
    private TimeManager TimeInstance;

    public Action OnWinLevel;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            Debug.Log("Manca il CanvasGroup!");
            return;
        }
        _canvasGroupFader = GetComponent<CanvasGroupFader>();
        if (_canvasGroupFader == null)
        {
            Debug.Log("Manca il CanvasGroupFader!");
            return;
        }

        _timer = FindAnyObjectByType<Timer>();
        if (_timer == null)
        {
            Debug.Log($"{gameObject.name} non è riuscito a trovare il Timer!");
        }
    }

    private void Start()
    {
        TimeInstance = TimeManager.Instance;
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroupFader.SetVisibilityState(false);

        gameObject.SetActive(false);
    }

    public void OnUICalled()
    {
        gameObject.SetActive(true);

        _canvasGroupFader.CallFadeIn(_canvasGroupFader.FadeTimer);
        OnWinLevel?.Invoke();
        _timerText.text = (_timer.CurrentTime - _timer.MaxTime ).ToString();
        SaveSystem.Save(new SaveData());
    }
    public void GoToNextLevel() => Debug.Log("I'm sorry, this is the only level available at the moment!");
    
    public void RetryLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
