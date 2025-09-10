using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private int _maxTime = 60;
    [SerializeField] private TextMeshProUGUI _timerCountdown;

    public UnityEvent<int> OnTimerChange;
    public Action OnTimerEnd;
    public Action<int> OnTimerStart;

    private TimeManager TimeInstance;

    private float _timer;
    private int _lastSecond;
    private bool _isRunning;
    private int _currentTime;
    
    public int CurrentTime => _currentTime;
    public int MaxTime => _maxTime;
    public float TimeRemaining => _timer;
    public bool IsRunning => _isRunning;
    public float ElapsedTime => _maxTime - _timer;

    public void Start()
    {
        TimeInstance = TimeManager.Instance;
        if (TimeInstance == null)
        {
            Debug.LogError("TimeManager instance not found in the scene.");
            return;
        }
        if (!TimeInstance.IsRunning)
        {
            TimeInstance.SetTimeScaleToStandard();
            StartTimer();
        }
    }

    private void Update()
    {
        if (!TimeInstance.IsRunning) return;

        // Pausa il _fadeTimer se il gioco è in pausa

        _timer -= Time.deltaTime;
        _currentTime = Mathf.CeilToInt(_timer);

        if (_currentTime != _lastSecond)
        {
            _lastSecond = _currentTime;
            OnTimerChange?.Invoke(_currentTime);
            DisplayTimer();
        }

        if (_timer <= -1f)
        {
            StopTimer();
            OnTimerEnd?.Invoke();
        }
    }

    public void StartTimer()
    {
        _timer = _maxTime;
        _lastSecond = Mathf.CeilToInt(_timer);
        _currentTime = _lastSecond;
        _isRunning = true;

        OnTimerStart?.Invoke(_maxTime);
        OnTimerChange?.Invoke(_lastSecond);
        DisplayTimer();
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void PauseTimer()
    {
        _isRunning = false;
        Time.timeScale = 0f;
    }

    public void ResumeTimer()
    {
        if (_timer > 0f)
        {
            _isRunning = true;
            Time.timeScale = 1f;
        }
    }

    public void ResetTimer()
    {
        _timer = _maxTime;
        _currentTime = Mathf.CeilToInt(_timer);
        _lastSecond = _currentTime;
        _isRunning = false;

        OnTimerChange?.Invoke(_currentTime);
        DisplayTimer();
    }


    public void SetMaxTime(int maxTime)
    {
        if (this._maxTime == maxTime) return;
        if (maxTime < 1)
        {
            Debug.LogWarning("Error: Timer must be of at least 1 second!");
            return;
        }
        this._maxTime = maxTime;
        ResetTimer();
    }

    public void DisplayTimer()
    {
        if (_timerCountdown == null)
        {
            Debug.LogWarning("Timer text component non assegnato!");
            return;
        }

        int totalTime = _currentTime;
        int minutes = totalTime / 60;
        int seconds = totalTime % 60;

        string displayedTime = minutes > 0 ? $"{minutes:00}:{seconds:00}" : $"{seconds:00}";
        if (_timerCountdown.text.Equals(displayedTime))
            return;
        else
            SetTimerText(displayedTime);

    }

    public void SetTimerText(string text)
    {
        if (_timerCountdown == null)
        {
            Debug.LogWarning("Timer text component non assegnato!");
            return;
        }
        int textToInt;
        if (int.TryParse(text, out textToInt) && textToInt <= 0)
        {
            text = "0";
        }
        _timerCountdown.text = text;
    }
}
