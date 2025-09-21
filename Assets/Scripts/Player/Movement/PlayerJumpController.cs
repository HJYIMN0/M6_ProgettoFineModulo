using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerJumpController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _maxJumpForce = 20f;
    [SerializeField] private float _secondJumpForce = 15f;
    [SerializeField] private float _maxChargeTime = 2f; // Tempo massimo per caricare il salto

    [Header ("Time Settings")]
    [SerializeField] private float _timeWhileCharging = 0.3f;


    private TimeManager timeManager;
    private PlayerInput playerInput;
    private PlayerController playerController;
    private Rigidbody _rb;

    // Jump state
    private bool _isChargingJump = false;
    private float _chargeTime = 0f;
    private float _currentJumpForce = 0f;
    private Vector3 _originalVelocity;

    // Second jump
    public bool _hasSecondJump { get; set; } = false;
    public UnityEvent<bool> _onSecondJump;
    public Action OnMaxJumpCharge;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();
        _rb = GetComponentInParent<Rigidbody>();

        if (playerInput == null) Debug.LogError($"Missing the PlayerInput component on {this.gameObject.name}");
        if (playerController == null) Debug.LogError($"Missing the PlayerController component on {this.gameObject.name}");
        if (_rb == null) Debug.LogError($"Missing the Rigidbody component on {this.gameObject.name}");
    }

    private void Start()
    {
        timeManager = TimeManager.Instance;
        if (timeManager == null) Debug.LogError($"Time Manager not found by {this.gameObject.name}");
    }

    private void Update()
    {
        HandleJumpInput();

        // Reset second jump when grounded
        if (playerInput.IsGrounded())
        {
            SetSecondJump(false);
        }
    }

    private void HandleJumpInput()
    {
        // Start charging jump
        if (playerInput.IsGrounded() && Input.GetButtonDown("Jump"))
        {
            StartChargeJump();
        }

        // Continue charging jump
        else if (playerInput.IsGrounded() && Input.GetButton("Jump") && _isChargingJump)
        {
            ChargeJump();
        }

        // Release jump
        else if (playerInput.IsGrounded() && Input.GetButtonUp("Jump") && _isChargingJump)
        {
            ExecuteJump();
        }

        // Second jump (in air)
        else if (!playerInput.IsGrounded() && _hasSecondJump && Input.GetButtonDown("Jump"))
        {
            ExecuteSecondJump();
        }

        // Cancel jump if not grounded anymore while charging
        else if (!playerInput.IsGrounded() && _isChargingJump)
        {
            CancelChargeJump();
        }
    }

    private void StartChargeJump()
    {

        OnMaxJumpCharge?.Invoke();
        _isChargingJump = true;
        _chargeTime = 0f;
        _currentJumpForce = _jumpForce;

        // Salva la velocità originale e rallenta il tempo
        _originalVelocity = _rb.velocity;
        timeManager.SetTimeScale(_timeWhileCharging);

        // Disabilita il movimento del player controller durante la carica
        playerController.SetMovementEnabled(false);

        Debug.Log("Started charging jump");
    }

    private void ChargeJump()
    {        
        _chargeTime += Time.unscaledDeltaTime;

        float chargeProgress = Mathf.Clamp01(_chargeTime / _maxChargeTime);
        _currentJumpForce = Mathf.Lerp(_jumpForce, _maxJumpForce, chargeProgress);

        // Rallenta progressivamente il player mantenendo la direzione
        Vector3 horizontalVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        Vector3 deceleratedVelocity = Vector3.MoveTowards
            (horizontalVelocity, Vector3.zero, playerController._speedDeceleration * Time.fixedDeltaTime);
        _rb.velocity = new Vector3(deceleratedVelocity.x, _rb.velocity.y, deceleratedVelocity.z);
    }
  


    private void ExecuteJump()
    {
        if (!_isChargingJump) return;
            
        // Esegui il salto con la forza calcolata
        _rb.velocity = new Vector3(_rb.velocity.x, _currentJumpForce, _rb.velocity.z);

        // Reset dello stato
        FinishJumpCharge();

        Debug.Log($"Jump executed with force: {_currentJumpForce:F1}");
    }

    private void ExecuteSecondJump()
    {
        if (!_hasSecondJump) return;

        _rb.velocity = new Vector3(_rb.velocity.x, _secondJumpForce, _rb.velocity.z);
        SetSecondJump(false);

        Debug.Log("Second jump executed");
    }

    private void CancelChargeJump()
    {
        if (!_isChargingJump) return;

        FinishJumpCharge();
        Debug.Log("Jump charge cancelled");
    }

    private void FinishJumpCharge()
    {
        _isChargingJump = false;
        _chargeTime = 0f;
        _currentJumpForce = _jumpForce;

        // Ripristina il tempo normale e riattiva il movimento
        timeManager.SetTimeScale(1f);
        playerController.SetMovementEnabled(true);
    }

    public void SetSecondJump(bool value)
    {
        if (_hasSecondJump == value) return;

        _hasSecondJump = value;
        _onSecondJump?.Invoke(value);

        if (value)
        {
            Debug.Log("Second jump enabled");
        }
    }

    // Getters per debug/UI
    public bool IsChargingJump => _isChargingJump;
    public float ChargeProgress => _chargeTime / _maxChargeTime;
    public float CurrentJumpForce => _currentJumpForce;
}
