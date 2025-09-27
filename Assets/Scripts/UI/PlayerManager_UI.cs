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

    private GameManager _gameManager;
    

    private void Start()
    {
        _gameManager = GameManager.Instance;

        _gameManager.Player.gameObject.SetActive(true);
        _gameManager.Player.GetComponentInParent<LifeController>().OnLifeChanged += OnLifeChanged;
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
