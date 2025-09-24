using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosingTrigger : MonoBehaviour
{
    private Timer _timer;
    private CanvasGroup _canva;
    private CanvasGroupFader _fader;
    private TimeManager _timerInstance;
    private GameManager _gameManager;

    public Action OnLose;

    private void Awake()
    {

        _fader = GetComponent<CanvasGroupFader>();
        if (_fader == null)
            Debug.LogError($"CanvasGroupFader component not found on {gameObject.name}.");

        _canva = GetComponent<CanvasGroup>();
        if (_canva == null)
            Debug.LogError($"CanvasGroup component not found on {gameObject.name}.");
        else
        {
            _canva.alpha = 0;
            _canva.interactable = false;
            _canva.blocksRaycasts = false;
        }
    }

    private void Start()
    {
        _timerInstance = TimeManager.Instance;
        _gameManager = GameManager.Instance;

        if (_timerInstance == null)
            Debug.LogError("TimeManager instance not found in the scene.");

        _timer = FindAnyObjectByType<Timer>();
        if (_timer == null)
            Debug.LogError($"Timer component not found on {gameObject.name}.");
        else
            _timer.OnTimerEnd += TriggerLoseUI;

        if (_gameManager != null)
        {
            LifeController playerlife = _gameManager.Player.GetComponentInParent<LifeController>();
            if (playerlife != null)
            {
                playerlife._onDeath += TriggerLoseUI;
            }
            else
            {
                Debug.LogWarning($"{this.gameObject.name} couldnt' find the LifeController on {_gameManager.Player.name}!");                
            }
        }
        else Debug.LogWarning($"Missing GameManager Instance on {this.gameObject.name}");

        gameObject.SetActive( false );
    }

    private void Update()
    {
        if (_fader._isVisible)
        {
            gameObject.SetActive( true );
            _timerInstance.SetTimeScale(0f);
        }
    }

    public void TriggerLoseUI()
    {
        gameObject.SetActive(true);
        OnLose?.Invoke();
        _fader.CallFadeIn(_fader.FadeTimer);
    }
}
