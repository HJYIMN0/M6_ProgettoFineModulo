using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosingTrigger : MonoBehaviour
{
    private Timer _timer;
    private CanvasGroup _canva;
    private CanvasGroupFader _fader;
    private TimeManager _timerInstance;

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
        if (_timerInstance == null)
            Debug.LogError("TimeManager instance not found in the scene.");

        _timer = FindAnyObjectByType<Timer>();
        if (_timer == null)
            Debug.LogError($"Timer component not found on {gameObject.name}.");
        else
            _timer.OnTimerEnd += TriggerLoseUI;
    }

    private void Update()
    {
        if (_fader._isVisible)
        {
            _timerInstance.SetTimeScale(0f);
        }
    }

    public void TriggerLoseUI()
    {
        _fader.CallFadeIn(_fader.FadeTimer);
    }
}
