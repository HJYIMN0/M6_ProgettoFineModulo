using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float _groundCheckRadius = 0.5f;
    [SerializeField] private GameObject _groundCheckPosition;
    [SerializeField] private LayerMask _groundLayer;

    private Vector3 _movement;
    public Vector3 Movement => _movement;

    private void Update()
    {
        GetInput();
    }

    #region MovementInput
    public void GetInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            _movement = new Vector3(h, 0, v);
        }
        else
        {
            _movement = Vector3.zero; 
        }
    }
    #endregion

    public bool IsRunning()
    {
        return Input.GetButton("Fire3");
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheckPosition.transform.position, _groundCheckRadius, _groundLayer);
    }

    #region JumpLogic
    public bool IsTryingToJump()
    {
        return Input.GetButton("Jump") && IsGrounded();
    }

    public bool IsJumping()
    {
        // CORREZIONE: logica semplificata per il salto
        return Input.GetButtonDown("Jump") && IsGrounded();
    }
    #endregion


    private void OnDrawGizmos()
    {
        if (_groundCheckPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheckPosition.transform.position, _groundCheckRadius);
        }
    }
}
