using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class PlayerManager_UI : MonoBehaviour
{
    [SerializeField] private Image[] _hp;
    [SerializeField] private Image _hasDoubleJump;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private int _maxScore = 10;

    public UnityEvent _onAllCoinsCollected;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        foreach (Image image in _hp)
            image.color = Color.green;

        _scoreText.text = $" 0/{_maxScore}";
    }


    public void TurnOffHp()
    {
        foreach (Image hp in _hp)
        {
            if (hp != null)
            {
                if (hp.color != Color.red)
                {
                    hp.color = Color.red;
                    return;
                }
            }
        }
    }

    public void UiDoubleJump(bool hasDoubleJump)
    {        
        if (_hasDoubleJump == null) return;
        if (hasDoubleJump)
        {
            _hasDoubleJump.color = new Color(0, 1, 0, 1);
        }
        else
        {
            _hasDoubleJump.color = new Color(0, 1, 0, 0);
            
        }

    }

    public void CollectedCoins(int score)
    {
        _scoreText.text = $"{score} / {_maxScore}";
        if (score >= _maxScore)
        {
            Debug.Log("Preso tutte le monete!");
            score = _maxScore;
            _onAllCoinsCollected?.Invoke();            
        }

    }

    public void CallFadeOut()
    {
        StartCoroutine(FadeOut());
        _canvasGroup.interactable = false;
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

    //private void Update()
    //{
    //    TurnOffHp();
    //}


}
