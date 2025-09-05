using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ResetScene : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _startingPos;
    [SerializeField] private int _damageOnReset = 1;
    [SerializeField] private float _fallThreshold = -10f; // Threshold for falling
    private Rigidbody _playerRb;
    private LifeController _playerHP;
    private bool _isAlive;

    [SerializeField] public float _maxTimer = 60;
    public UnityEvent <int> _onTimerChange;
    public UnityEvent _onTimerEnd;
    private float _timer = 0;
    private int _lastSecond = -1;


    private void Awake()
    {
        if (_player == null)
        {
            Debug.LogWarning("Errore! Nessun Player trovato!");
            _player = GameObject.FindGameObjectWithTag("Player");
        }    


        _playerRb = _player.GetComponent<Rigidbody>();
        if (_playerRb == null)
        {
            Debug.LogError("Rigidbody component is missing on the player object.");
        }
        _playerHP = _player.GetComponentInChildren<LifeController>();
        if (_playerHP == null)
        {
            Debug.LogError("LifeController component is missing on the player object.");
        }
    }

    private void Start()
    {
        if (_startingPos == Vector3.zero)
        {
            _startingPos = _player.transform.position;
        }
    }

    private void Update()
    {
        IsPlayerFalling();


        _timer += Time.deltaTime;

        int currentSecond = Mathf.FloorToInt(_timer);
        if (currentSecond != _lastSecond)
        {
            _lastSecond = currentSecond;
            _onTimerChange?.Invoke(_lastSecond);
        }

        if (_timer >= _maxTimer)
        {
            Time.timeScale = 0f;
            _onTimerEnd?.Invoke();
        }

    }

    public void IsPlayerFalling()
    {
        if (_player.transform.position.y < _fallThreshold) 
        {
            Debug.Log("Player is falling.");
            ResetPlayerPosition();
        }

    }

    public void ResetPlayerPosition()
    {
        if (IsALive())
        {
            _player.transform.position = _startingPos;
            _playerRb.velocity = Vector3.zero; // Reset velocity
            _playerRb.angularVelocity = Vector3.zero; // Reset angular velocity            
            _playerHP.TakeDamage(_damageOnReset);
            Debug.Log("Player position reset and damage applied.");
        }
        //else
        //{
        //    _playerHP.Die(_player);
        //}
    }

    public bool IsALive()
    {
        if (_playerHP.GetHp() > 0)
        { _isAlive = true; }

        else
        { _isAlive = false; }

        return _isAlive;
    }

    public void SetStartingPos(Vector3 startingPos) => _startingPos = startingPos;
}
