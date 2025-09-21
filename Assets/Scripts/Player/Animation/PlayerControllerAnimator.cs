using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerControllerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private LifeController _lifeController;
    
    private bool isCoroutineRunning;

    void Awake()
    {
        if (_anim == null || _playerInput == null || _lifeController == null || _rb == null)
        {
            Debug.LogError($"You Forgot to assign the references from the inspecor in {this.gameObject.name}");
        }        
    }

    void Update()
    {
        float xMove = _playerInput.IsTryingToJump() ? 0 : _playerInput.Movement.magnitude;
        
        _anim.SetFloat("xMovement", xMove);
        _anim.SetFloat("yMovement", _rb.velocity.y);

        _anim.SetBool("Charging", _playerInput.IsTryingToJump());
        _anim.SetBool("IsGrounded", _playerInput.IsGrounded());

        Debug.Log(_anim.GetFloat("xMovement"));
        Debug.Log(_anim.GetBool("Charging"));
    } 
}
