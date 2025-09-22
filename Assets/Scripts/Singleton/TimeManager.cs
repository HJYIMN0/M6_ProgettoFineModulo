using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
public class TimeManager : AbstractSingleton<TimeManager>
{
    [SerializeField] private float _standardTimeScale = 1f;
    [SerializeField] private string[] _scenesToSkipDontDestroy = { };


    private bool _isRunning;
    private float _timeScale = 1f;
    private float _lastTimeScale;

    private bool _isCoroutineRunning;

    public float StandardTimeScale => _standardTimeScale;
    public float TimeScale => _timeScale;
    public bool IsRunning => _isRunning;

    public Action<float> OnTimeScaleChanged;


    public override bool IsDestroyedOnLoad() => false;
    public override bool ShouldDetatchFromParent() => true;

    private void Start()
    {
        foreach (string sceneName in _scenesToSkipDontDestroy)
        {
            if (SceneManager.GetActiveScene().name.Equals(sceneName, StringComparison.OrdinalIgnoreCase))
            {
                Destroy(gameObject);
                return;
            }
        }
    }
    private void Update()
    {
        _isRunning = !Time.timeScale.Equals(0f);
        if (!_timeScale.Equals(Time.timeScale))
        {
            Time.timeScale = _timeScale;
            OnTimeScaleChanged?.Invoke(_timeScale);
        }
    }

    public void PauseGame()
    {
        if (!_isRunning) return;

        _lastTimeScale = _timeScale; // Salva il valore corrente
        _timeScale = 0f;
    }


    public void ResumeGame()
    {
        if (_isRunning) return;

        _timeScale = _lastTimeScale; // Ripristina il valore precedente
    }


    public void SetTimeScale(float scale)
    {
        _timeScale = scale;
        _lastTimeScale = scale;
    }

    public void SetTimeScaleToStandard()
    {
        _timeScale = _standardTimeScale;
        _lastTimeScale = _timeScale;
    }

    public void CallTransitionTimeScale(float from, float to, float duration)
    {
        if (_isCoroutineRunning) return;
        StartCoroutine(TransitionTimeScale(from, to, duration));
    }

    private IEnumerator TransitionTimeScale(float from, float to, float duration)
    {
        _isCoroutineRunning = true;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / duration;
            SetTimeScale(Mathf.Lerp(from, to, t));
            Debug.Log($"timescale = {Time.timeScale}");
            yield return null;
        }  
        _isCoroutineRunning = false;
    }

    public void StopAllTransitions()
    {
        _isCoroutineRunning = false;
        StopAllCoroutines();
    }
}