using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _increasedSpeed = 2; // Speed when running
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _secondJumpForce = 15f;
    [SerializeField] private float _groundCheckRadius = 0.5f;
    [SerializeField] private GameObject _groundCheckPosition;
    [SerializeField] private LayerMask _groundLayer; 

    private float h;
    private float v;
    private Transform _cameraTransform;
    private Rigidbody _rb;

    public bool _isJumping { get; private set; } = false;
    public bool _hasSecondJump { get; set; }
    public bool _isMoving { get; private set; }

    public bool _isRunning { get; private set; }

    public UnityEvent<bool> _onSecondJump;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
    }

    public void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if (IsGrounded())
        {
            SetSecondJump(false); // Reset second jump when grounded
        }
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _isJumping = true;
        }
        else if (!IsGrounded () && Input.GetButtonDown("Jump"))
        {
            SecondJump();
        }
        if (Input.GetButtonDown("Fire3"))
        {
            _isRunning = true;
            _speed *= _increasedSpeed; // Increase speed when running
        }
        if (Input.GetButtonUp("Fire3"))
        {
            _isRunning = false;
            _speed /= _increasedSpeed; // Reset speed when not running
        }

        //if (Input.GetButtonDown("Jump") && _hasSecondJump == true)
        //{
        //    //Debug.Log("Has second jump: " + _hasSecondJump);
        //    //_isJumping = true;
        //    //Jump();
        //    //_hasSecondJump = false; // Disable second jump after use
        //}
        //Debug.Log("SecondJump: " + _hasSecondJump);
        //Debug.Log($"Is Grounded: {IsGrounded()}");
    }

    public void SecondJump()
    {
        if (!_hasSecondJump) return;
        else 
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
            _hasSecondJump = false; // Disable second jump after use
            Debug.Log("Second jump executed");
        }
    }

    public void SetSecondJump(bool value)
    {
        _hasSecondJump = value;
        _onSecondJump?.Invoke(value);
        if (value == true)
        {
            Debug.Log("Second jump enabled");
        }
    }

    private void FixedUpdate()
    {
        Move();
        if (_isJumping)
        {
            Jump();
            _isJumping = false;
        }
    }

    public void Move()
    {
        // Ottieni la direzione in base alla camera
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;

        // Ignora la componente verticale
        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // Costruisci direzione di movimento relativa alla camera
        Vector3 direction = (camForward * v + camRight * h).normalized;

        // Se ci stiamo muovendo
        if (direction.magnitude > 0.1f)
        {
            _rb.velocity = new Vector3(direction.x * _speed, _rb.velocity.y, direction.z * _speed);
            _isMoving = true;

            // Ruota gradualmente verso la direzione di movimento
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
        else
        {
            _rb.angularVelocity = Vector3.zero;
            _isMoving = false; // Non ci stiamo muovendo
        }
    }

    public bool IsGrounded()
    {
        if ( Physics.CheckSphere(_groundCheckPosition.transform.position, _groundCheckRadius, _groundLayer) )  return true;
        else return false;
    }

    public void Jump()
    {
        if (!_hasSecondJump)
        { 
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
            Debug.Log("im jumping");
        }
        else if (_hasSecondJump)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _secondJumpForce, _rb.velocity.z);
            Debug.Log("im second jumping");
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheckPosition.transform.position, _groundCheckRadius);
    }

    public float GetSpeed() => _speed;
}
