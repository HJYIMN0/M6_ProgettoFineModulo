using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using Unity.VisualScripting;
using System.Timers;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class WinningUI : MonoBehaviour
{    
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private CountdownTimer _timer;

    private CanvasGroup _canvasGroup;

    public UnityEvent _OnWinLevel;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null) Debug.Log("Manca il CanvasGroup!");
    }

    private void Start()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
    }


    public void CallWinUI()
    {
        StartCoroutine("FadeIn");
        _canvasGroup.interactable = true;

        float timer = _timer.GetTime();        
        string timerString = timer.ToString("F3");
        _timerText.text = $"Il tuo tempo: {timerString}";

    }

    private IEnumerator FadeIn()
    {
        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    public void GoToNextLevel() => Debug.Log("I'm sorry, this is the only level available at the moment!");
    
    public void RetryLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
