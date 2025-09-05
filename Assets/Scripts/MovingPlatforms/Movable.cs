using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Movable : MonoBehaviour
{

    [SerializeField] protected float _speed = 5f;
    [SerializeField] protected Rigidbody _rb;

    protected GameObject _player;
    protected Rigidbody _playerRb;
    protected PlayerController _playerController;
    public abstract void Move();

    public void SetPlayer(GameObject player)
    {
        if (player == null) return;
        if (!player.CompareTag("Player")) return;
        _player = player;
    }

    public GameObject GetPlayer() => _player;

    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)        
            Debug.LogError("Rigidbody component is missing on the IMovable object.");        
    }
}
