using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] public Func<int, int> _onCoinCollected;

    private bool _isCollected = false;
    //[SerializeField] private PlayerController _player;
    [SerializeField] private float _setActiveDelay = 2f;
    private CoinManager _coinManager;

    private void Start()
    {
        _isCollected = false;
        _coinManager = FindObjectOfType<CoinManager>();
        if (_coinManager == null)
        {
            Debug.LogError($"{gameObject.name} can't find CoinManager in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerController>().SetSecondJump(true);
            other.GetComponentInParent<PlayerController>()._onSecondJump?.Invoke(true);
            if (!_isCollected)
            {
                _coinManager?.IncreaseCollectedCoinsByValue(value);
                Debug.Log("Coins Collected: " + _coinManager?.collectedCoins);
            }
            _isCollected = true;

            _onCoinCollected?.Invoke(value);
            gameObject.SetActive(false);
            Debug.Log("Taken!");
        }
    }

    private void OnDisable()
    {
        Invoke("SetActiveTrue", _setActiveDelay);
    }

    public void SetActiveTrue()
    {
        gameObject.SetActive(true);
    }

    //private void Update()
    //{
    //    Debug.Log(value);
    //    Debug.Log($"score uguale a {score}");
    //}
}
