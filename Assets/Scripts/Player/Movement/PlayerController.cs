using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _baseSpeed = 5f;
    [SerializeField] public float _speedMultiplier = 2f; // Speed when running
    [SerializeField] public float _speedDeceleration = 2f;
    [SerializeField] private float _rotationSpeed = 10f;

    private Transform _cameraTransform;
    private Rigidbody _rb;
    private PlayerInput _playerInput;

    // Movement state
    public bool _isMoving { get; private set; }
    public bool _isRunning { get; private set; }

    // Control flags
    private bool _movementEnabled = true;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
        _playerInput = GetComponent<PlayerInput>();

        if (_rb == null) Debug.LogError($"{this.gameObject.name} missing RigidBody Component!");
        if (_cameraTransform == null) Debug.LogError($"{this.gameObject.name} missing the Camera Transform!");
        if (_playerInput == null) Debug.LogError($"{this.gameObject.name} missing PlayerInput component!");
    }

    private void FixedUpdate()
    {
        if (_movementEnabled)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 direction = Vector3.zero;
        _isMoving = _playerInput.Movement != Vector3.zero;
        _isRunning = _playerInput.IsRunning();

        // Movimento relativo alla camera
        if (_playerInput.Movement != Vector3.zero)
        {
            Vector3 camForward = _cameraTransform.forward;
            Vector3 camRight = _cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            direction = (_playerInput.Movement.x * camRight +
                         _playerInput.Movement.z * camForward).normalized;

            // Rotazione fluida
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

            // Calcola velocità con corsa
            float finalSpeed = _baseSpeed;
            if (_playerInput.IsRunning()) finalSpeed *= _speedMultiplier;

            Vector3 finalVelocity = direction * finalSpeed;
            finalVelocity.y = _rb.velocity.y; // conserva velocità verticale

            _rb.velocity = finalVelocity;
        }
        else
        {
            // Stop completo quando non c'è input
            // Decelera la velocità orizzontale fino a zero
            Vector3 horizontalVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            Vector3 deceleratedVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, _speedDeceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector3(deceleratedVelocity.x, _rb.velocity.y, deceleratedVelocity.z);

            // Decelera la velocità angolare fino a zero
            _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, Vector3.zero, _speedDeceleration * Time.fixedDeltaTime);
        }
    }

    // Metodo per abilitare/disabilitare il movimento (usato da PlayerJumpController)
    public void SetMovementEnabled(bool value)
    {
        _movementEnabled = value;
    }

    // Getters
    public float GetSpeed() => _baseSpeed;
    public bool IsMovementEnabled => _movementEnabled;
}
