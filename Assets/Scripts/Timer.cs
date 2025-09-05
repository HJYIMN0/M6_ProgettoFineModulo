using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private int maxTime = 60;
    [SerializeField] private TextMeshProUGUI _TimerCountdown;
    //[SerializeField] private textmeshpro _timerCountdown;
    private float _timer;
    private int _lastSecond;
    private bool _isRunning;
    private int _currentTime;

    public UnityEvent<int> _onTimerChange;
    public UnityEvent _onTimerEnd;

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (!_isRunning) return;

        _timer -= Time.deltaTime;
        _currentTime = Mathf.CeilToInt(_timer);

        if (_currentTime != _lastSecond)
        {
            _onTimerChange?.Invoke(_currentTime);
            _lastSecond = _currentTime;
            //Debug.Log(_currentTime);
        }

        if (_timer <= -1f)
        {
            _isRunning = false;
            _onTimerEnd?.Invoke();
        }
    }

    public void StartTimer()
    {
        _timer = maxTime;
        _lastSecond = Mathf.CeilToInt(_timer);
        _isRunning = true;

        _onTimerChange?.Invoke(_lastSecond); // Invoca subito all'inizio
    }

    public void CallFadeOut()
    {
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        if (_canvasGroup != null)
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            Debug.Log("Manca il CanvasGroup da qualche parte!");
        }
    }

    public float GetTime() => maxTime - _currentTime;


    public void DisplayTimer()
    {
        int totalTime = _currentTime;
        int minutes = totalTime / 60;
        int seconds = totalTime % 60;

        string displayedTime = minutes > 0 ? $"{minutes}:{seconds:00}" : $"{seconds}";
        _TimerCountdown.text = displayedTime;
        //Debug.Log(_TimerCountdown.text);
    }


    //public void DisplayTimer()
    //{
    //    int displayTimer = _currentTime;
    //    _TimerCountdown.text = _currentTime.ToString();
    //    //Debug.Log(_TimerCountdown.text);
    //}

    //public int DisplayTime(float currentTime)
    //{
    //    int timer = Mathf.RoundToInt(_maxTime - currentTime);
    //    return timer;        
    //}
}