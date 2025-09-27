using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;


public class PlayerManager_UI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image[] _healthImages;

    [Header("Sprites")]
    [SerializeField] private Sprite _hpSpriteOn;
    [SerializeField] private Sprite _hpSpriteOff;

    [Header("DoubleJump")]
    [SerializeField] private GameObject _jumpSprite;

    [Header("CoinCollection")]
    [SerializeField] private TextMeshProUGUI _coinText;
    
    private GameManager _gameManager;
    private CoinManager _coinManager;
    

    private void Start()
    {
        _gameManager = GameManager.Instance;

        _gameManager.Player.gameObject.SetActive(true);
        _gameManager.Player.GetComponentInParent<LifeController>().OnLifeChanged += OnLifeChanged;

        _coinManager = _gameManager.CoinManager;
        _coinManager.OnCoinCollected += DisplayCoin;

        DisplayCoin(_coinManager.CollectedCoins, _coinManager.totalCoins);

        _gameManager.Player.GetComponentInParent<PlayerJumpController>().OnSecondJump += ShowDoubleJumpUI;
        _jumpSprite.SetActive(false);
    }
    public void OnLifeChanged(int currentHp, int maxHp)
    {
        if (_healthImages == null || _hpSpriteOn == null || _hpSpriteOff == null) return;

        int count = Mathf.Min(_healthImages.Length, maxHp);

        for (int i = 0; i < count; i++)
        {
            if (_healthImages[i] == null) continue;

            _healthImages[i].sprite = i < currentHp ? _hpSpriteOn : _hpSpriteOff;
        }
    }

    public void DisplayCoin(int collectedcoins, int totalCoins)
    {
        if (_gameManager.WinningTrigger.CollectedAllCoins)
        {
            _coinText.text = $"{_coinManager.totalCoins} / {_coinManager.totalCoins}";
            return;
        }
        _coinText.text = $"{collectedcoins} / {totalCoins}";
    }

    public void ShowDoubleJumpUI(bool value)
    {
        _jumpSprite.SetActive(value);
    }

    public void TestMethod()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log($"{_gameManager.Player.gameObject.name}");
            _gameManager.Player.GetComponentInParent<LifeController>().TakeDamage(1);
            int a = _gameManager.Player.GetComponentInParent<LifeController>().GetHp();
            int b = _gameManager.Player.GetComponentInParent<LifeController>().GetMaxHp();
            OnLifeChanged(a , b);
            Debug.Log("Doing it!");
        }
    }
}
