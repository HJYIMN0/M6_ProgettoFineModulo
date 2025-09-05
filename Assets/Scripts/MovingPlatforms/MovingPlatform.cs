using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MovingPlatform : Movable
{
    [SerializeField] private float _distance = 5f;
    [SerializeField] private float _timer = 2f;

    private Vector3 startPos;
    private Vector3 dir;
    private bool _isMovingForward = true;
    private bool _isWaiting = false;
    private float elapsedTime = 0f;

    public void Start()
    {
        startPos = transform.position;
    }

    public void FixedUpdate()
    {
        Move();
    }

    public virtual void OnCollisionStay(Collision collision)
    {
        if (_playerController._isMoving)
        {
            float newXSpeed = (_playerController.GetSpeed() + _speed) * _rb.velocity.x;
            float newZSpeed = (_playerController.GetSpeed() + _speed) * _rb.velocity.z;
            _playerRb.velocity = new Vector3(newXSpeed, _playerRb.velocity.y, newZSpeed); // Apply the platform's movement to the player
        }
        else
        {
            float newZSpeed = _rb.velocity.z;
            float newXSpeed = _rb.velocity.x;
            _playerRb.velocity = new Vector3(newXSpeed, _playerRb.velocity.y, newZSpeed); // Maintain the player's vertical velocity
        }
    }


    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player = collision.gameObject;
            Debug.Log($"Player {_player.name} salito sulla piattaforma");

            _playerRb = _player.GetComponent<Rigidbody>();
            if (_playerRb == null)
                Debug.LogError("Rigidbody component is missing on the player object.");

            _playerController = _player.GetComponentInChildren<PlayerController>();
            if (_playerController == null)
                Debug.LogError("PlayerController component is missing on the player object.");
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        _playerRb.velocity = _playerRb.velocity;
        _player = null;
        _playerRb = null;
        _playerController = null;
    }
    public override void Move()
    {
        if (_isWaiting)
        {
            elapsedTime += Time.fixedDeltaTime;

            if (elapsedTime >= _timer)
            {
                _isWaiting = false;       // Fine pausa
                elapsedTime = 0f;         
            }
            else
            {
                return; // NON muovere la piattaforma finché non passa il tempo
            }
        }

        if (_isMovingForward)
        {
            dir = transform.forward;
        }
        else
        {
            dir = - transform.forward;
        }

        _rb.MovePosition(transform.position + dir * _speed * Time.fixedDeltaTime);

        if (Vector3.Distance(startPos, transform.position) > _distance)
        {
            Debug.Log($"Piattaforma ha raggiunto il limite di distanza {_distance} unità.");

            _isWaiting = true;               
            startPos = transform.position;   
            _isMovingForward = !_isMovingForward;
        }
    }
}
