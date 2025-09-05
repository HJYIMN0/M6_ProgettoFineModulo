using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PushingObstacle : Movable
{
    [SerializeField] public float _strength { get; private set; } = 15f;
    [SerializeField] private float _returningSpeed = 5f;
    [SerializeField] private float _distance = 5f;
    [SerializeField] private float _timeBeforeMove = 0.5f;
    [SerializeField] private float _resetTimer = 2.5f;
    [SerializeField] private float _timeBeforeNewAction = 5f;
    //[SerializeField] private Vector3 _boxCheckerSize = new Vector3(5, 5, 5);
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _trigger;
    [SerializeField] private GameObject _trap;



    UnityEvent playerOnTrigger;
    private Vector3 _startPos;
    private Vector3 _targetPosition;
    private bool _isCoroutineRunning;
    public bool _isMoving { get; private set; }
    public bool _isPushing { get; private set; }
    private bool _isReturning;

    public override void Awake() 
    {
        _startPos = _rb.transform.position;
    }
    public void Start()
    {
        _rb.transform.position = _startPos;
        _trap = _rb.gameObject;
        //_trap.SetActive(false);
        _isCoroutineRunning = false;
        _isPushing = false;
        Debug.Log($"startPos: {_startPos}");
        
    }

    private void OnEnable()
    {
        _rb.transform.position = _startPos;
    }

    //public bool FoundPlayer()
    //{
    //    if (Physics.CheckBox(_trigger.transform.position, _boxCheckerSize, Quaternion.identity, _playerLayer))
    //    {
    //        Debug.Log($"Player Found!")
    //        return true;
    //    }
    //}

    public void OnTriggerEnter(Collider other)
    {
        //Collision collisionOther = other.GetComponentInChildren<Collision>();
        //if (collisionOther != null)
        //    base.OnCollisionEnter(collisionOther);
        //else            
        //    Debug.LogWarning("Collider does not have a Collision component.");

        if (other.CompareTag("Player"))
        {
            SetPlayer(other.gameObject);
            Debug.Log($"Player found! {other.name}");
            //_playerRb = other.GetComponentInParent<Rigidbody>();
            //if (_playerRb == null)
            //{
            //    Debug.LogError("Rigidbody component is missing on the player object.");
            //}

            if (_isCoroutineRunning) return; // Prevent starting a new coroutine if one is already running

            StopAllCoroutines();
            StartCoroutine(PushPlayer(GetPlayer()));
        }
    }
    public IEnumerator PushPlayer(GameObject player)
    {
        _isCoroutineRunning = true;
        playerOnTrigger?.Invoke();
        //_trap.SetActive(true);
        yield return new WaitForSeconds(_timeBeforeMove);
        Move();
        yield return new WaitUntil(() => !_isMoving);
        yield return new WaitForSeconds(_resetTimer);
        Reset();
        yield return new WaitForSeconds(_timeBeforeNewAction);
        yield return new WaitUntil(() => !_isReturning);
        //_trap.SetActive(false);
        Debug.Log($"Finito coroutine fino a fine");
        _isCoroutineRunning = false;
    }

    public override void Move()
    {
        _isPushing = true;
        _isMoving = true;
        _targetPosition = _startPos + transform.forward * _distance;

    }

    public void Reset()
    {
        //_rb.MovePosition(transform.position + _startPos * _speed * Time.deltaTime);
        //_rb.MovePosition(transform.position + _startPos * _speed * Time.deltaTime);
        _isReturning = true;
        Debug.Log($"Obstacle reset to start position: {_startPos}");

    }

    //public override void OnCollisionEnter(Collision collision)
    //{
    //    _playerRb.AddForce(transform.forward * _strength, ForceMode.Impulse);
    //}



    private void FixedUpdate()
    {
        if (_isMoving)
        {
            // Muovi gradualmente verso la destinazione
            Vector3 newPos = Vector3.MoveTowards(_trap.transform.position, _targetPosition, _speed * Time.fixedDeltaTime);
            _rb.MovePosition(newPos);

            // Se l'obiettivo è raggiunto (o quasi)
            if (Vector3.Distance(_trap.transform.position, _targetPosition) <= 0.01f)
            {
                Debug.Log($"Obstacle has reached the target distance: {_distance} units.");
                _isMoving = false;
                _isPushing = false;
            }
        }
        if (_isReturning)
        {
            // Muovi gradualmente verso la posizione iniziale
            Vector3 newPos = Vector3.MoveTowards(_rb.position, _startPos, _returningSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(newPos);
            // Se la posizione iniziale è raggiunta
            if (Vector3.Distance(_rb.transform.position, _startPos) <= 0.1f)
            {
                Debug.Log($"Obstacle has returned to the start position: {_startPos}.");
                _isReturning = false;
                _isMoving = false;
                _isPushing = false;
            }
        }
    }

    private void Update()
    {
        //Debug.Log($"ispushing = {_isPushing}");
    }
}
