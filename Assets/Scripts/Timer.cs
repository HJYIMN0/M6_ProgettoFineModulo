using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Timer : Singleton<Timer>
{
    [Header("Timer Settings")]
    [SerializeField] private int _maxTime = 60;
    [SerializeField] private TextMeshProUGUI _timerCountdown;

    [Header("Events")]
    public UnityEvent<int> OnTimerChange;
    public UnityEvent OnTimerEnd;
    public UnityEvent OnTimerStart;

    [Header("Fade Events")]
    public UnityEvent OnFadeOutRequested;
    public UnityEvent OnFadeInRequested;

    private float _timer;
    private int _lastSecond;
    private bool _isRunning;
    private int _currentTime;
    private PauseMenu_Ui _pauseMenu;

    #region Properties
    public int CurrentTime => _currentTime;
    public int MaxTime => _maxTime;
    public float TimeRemaining => _timer;
    public bool IsRunning => _isRunning;
    public float ElapsedTime => _maxTime - _timer;
    #endregion

    protected override void OnSingletonAwake()
    {
        // Logica specifica del singleton Timer
        // Non applica DontDestroyOnLoad per default, può essere personalizzato
    }

    private void Start()
    {
        // Cerca l'istanza del singleton PauseMenu
        if (PauseMenu_Ui.Instance != null)
        {
            _pauseMenu = PauseMenu_Ui.Instance;
        }
        else
        {
            Debug.LogWarning("[Timer] PauseMenu_Ui singleton non trovato!");
        }

        StartTimer();
    }

    private void Update()
    {
        if (!_isRunning) return;

        // Pausa il timer se il gioco è in pausa
        if (_pauseMenu != null && _pauseMenu.IsPaused) return;

        _timer -= Time.deltaTime;
        _currentTime = Mathf.CeilToInt(_timer);

        if (_currentTime != _lastSecond)
        {
            _lastSecond = _currentTime;
            OnTimerChange?.Invoke(_currentTime);
            DisplayTimer();
        }

        if (_timer <= 0f)
        {
            StopTimer();
            OnTimerEnd?.Invoke();
        }
    }

    #region Timer Control
    public void StartTimer()
    {
        _timer = _maxTime;
        _lastSecond = Mathf.CeilToInt(_timer);
        _currentTime = _lastSecond;
        _isRunning = true;

        OnTimerStart?.Invoke();
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
    }

    public void ResumeTimer()
    {
        if (_timer > 0f)
        {
            _isRunning = true;
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

    public void AddTime(int seconds)
    {
        _timer += seconds;
        _timer = Mathf.Max(0f, _timer);
    }

    public void SetMaxTime(int newMaxTime)
    {
        _maxTime = newMaxTime;
        ResetTimer();
    }
    #endregion

    #region Display
    public void DisplayTimer()
    {
        if (_timerCountdown == null)
        {
            Debug.LogWarning("[Timer] Timer text component non assegnato!");
            return;
        }

        int totalTime = _currentTime;
        int minutes = totalTime / 60;
        int seconds = totalTime % 60;

        string displayedTime = minutes > 0 ? $"{minutes:00}:{seconds:00}" : $"{seconds:00}";
        _timerCountdown.text = displayedTime;
    }
    #endregion

    #region Fade Control (tramite eventi)
    public void RequestFadeOut()
    {
        OnFadeOutRequested?.Invoke();
    }

    public void RequestFadeIn()
    {
        OnFadeInRequested?.Invoke();
    }
    #endregion

    #region Debug & Utility
    private void OnValidate()
    {
        _maxTime = Mathf.Max(1, _maxTime);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void DebugTimerInfo()
    {
        Debug.Log($"[Timer] Current: {_currentTime}s, Remaining: {_timer:F1}s, Running: {_isRunning}");
    }
    #endregion
}

// Classe di utilità per collegare automaticamente Timer e CanvasGroupFader
public class TimerUIConnector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroupFader _canvasGroupFader;

    private void Start()
    {
        // Collega automaticamente gli eventi se i componenti sono trovati
        if (Timer.Instance != null && _canvasGroupFader != null)
        {
            Timer.Instance.OnFadeOutRequested.AddListener(_canvasGroupFader.OnTimerFadeOutRequested);
            Timer.Instance.OnFadeInRequested.AddListener(_canvasGroupFader.OnTimerFadeInRequested);

            Debug.Log("[TimerUIConnector] Timer e CanvasGroupFader collegati automaticamente.");
        }
        else
        {
            Debug.LogWarning("[TimerUIConnector] Impossibile collegare Timer e CanvasGroupFader. Controlla i riferimenti.");
        }
    }

    private void OnDestroy()
    {
        // Pulisce i listener per evitare memory leak
        if (Timer.Instance != null && _canvasGroupFader != null)
        {
            Timer.Instance.OnFadeOutRequested.RemoveListener(_canvasGroupFader.OnTimerFadeOutRequested);
            Timer.Instance.OnFadeInRequested.RemoveListener(_canvasGroupFader.OnTimerFadeInRequested);
        }
    }
}