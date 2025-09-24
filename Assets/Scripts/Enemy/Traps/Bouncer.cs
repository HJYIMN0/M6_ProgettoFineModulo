using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Bouncer : AbstractEnemy, iMovable
{
    [SerializeField] private float _speed;
    [SerializeField] private float _pushStrength = 2f;
    [SerializeField] private float _timeBeforeActivating = 2f;
    [SerializeField] private float _timeBeforeStartingAgain;
    
    [Range(1f, 5f)][SerializeField] private float _maxScale;

    private float _timer = 0f;
    private float _initialScale = 1f;
    private bool _isCoroutineRunning = false;
    private bool _isMoving = false;

    private void Start()
    {
        _initialScale = transform.localScale.y;
    }


    private void Update()
    {
        if (_isCoroutineRunning) return;

        if (IsPlayerInRange(out GameObject player))
        {
            Shrink();

            _timer += Time.deltaTime;
            if (_timer > _timeBeforeActivating)
            {
                //if (_isMoving) return;
                Move(transform.position, player.transform.position, _speed);
            }
        }
        else
        {
            float desiredScale = Mathf.Lerp(transform.localScale.y, _initialScale, _speed * Time.deltaTime);
            transform.localScale = new Vector3(_initialScale , desiredScale, _initialScale);
        }
    }

    public void Shrink()
    {
        if (_isMoving) return;
        if (_isCoroutineRunning) return;
        
        float desiredScale = Mathf.Lerp(transform.localScale.y, transform.localScale.y / 2, _timeBeforeActivating * Time.deltaTime);

        Vector3 scale = new Vector3(transform.localScale.x, desiredScale, transform.localScale.z);
        transform.localScale = scale;        
    }

    public void Move(Vector3 pos, Vector3 dir, float speed)
    {
        _isMoving = true;
        float desiredScale = Mathf.Lerp(transform.localScale.y, _maxScale, Time.deltaTime * speed);
        transform.localScale = new Vector3(transform.localScale.x, desiredScale, transform.localScale.z);

        if (!_isCoroutineRunning && Mathf.Abs(transform.localScale.y - _maxScale) < 0.01f)
        {
            StartCoroutine(ReturnToPosition());
            _timer = 0f;
        }
    }

    public IEnumerator ReturnToPosition()
    {
        _isCoroutineRunning = true;
        _isMoving = false;

        while (Mathf.Abs(transform.localScale.y - _initialScale) > 0.01f)
        {
            float desiredScale = Mathf.Lerp(transform.localScale.y, _initialScale, Time.deltaTime * _speed);
            transform.localScale = new Vector3(transform.localScale.x, desiredScale, transform.localScale.z);
            yield return null;
        }
        transform.localScale = new Vector3(transform.localScale.x, _initialScale, transform.localScale.z);
        yield return new WaitForSeconds(_timeBeforeStartingAgain);
        
        _isCoroutineRunning = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isMoving) return;

        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponentInParent<Rigidbody>();
            Debug.Log("isMoving " + _isMoving);
            if (playerRb != null)
            {
                Debug.Log("RIgidBody found!");
                playerRb.AddForce(transform.up * _pushStrength, ForceMode.Impulse);
            }
        }
    }

}