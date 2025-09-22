using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] public Func<int, int> _onCoinCollected;
    private bool _isCollected = false;
    //[SerializeField] private PlayerController _player;
    [SerializeField] private float _setActiveDelay = 2f;
    [SerializeField] private float _rotationSpeed = 100f;

    private GameManager GameManager => GameManager.Instance;
    private CoinManager _coinManager => GameManager.CoinManager;

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }

    private void Start()
    {
        if (GameManager == null)
        {
            Debug.LogError($"{this.gameObject.name} could not find the GameManager!");
            return;
        }
        _isCollected = false;
        if (_coinManager == null)
        {
            Debug.LogError($"{gameObject.name} can't find CoinManager in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerJumpController>().SetSecondJump(true);
            other.GetComponentInParent<PlayerJumpController>()._onSecondJump?.Invoke(true);

            if (!_isCollected)
            {
                _coinManager?.IncreaseCollectedCoinsByValue(value);
            }

            _isCollected = true;
            _onCoinCollected?.Invoke(value);
            gameObject.SetActive(false);
            Debug.Log("Coin Taken!");
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
}