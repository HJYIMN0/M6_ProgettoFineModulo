using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerControllerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private PlayerController _player;
    [SerializeField] private LifeController _lifeController;



    void Awake()
    {

        _anim = GetComponentInChildren<Animator>();
        if (_anim == null)
        {
            
            Debug.Log("Manca l'animator!");
        }


        if (_player == null)
            _player = GetComponent<PlayerController>();

        if (_lifeController == null)
                _lifeController = GetComponent<LifeController>();


    }

    void Update()
    {
        if (_player._isMoving)
            _anim.SetBool("IsMoving", true);
        else
            _anim.SetBool("IsMoving", false);

        if (_player._isJumping)
            _anim.SetBool("IsJumping", true);
        else
            _anim.SetBool("IsJumping", true);

        if (_player._isRunning)
            _anim.SetBool("IsRunning", true);
        else
            _anim.SetBool("IsRunning", false);

        Debug.Log("Walk " + _anim.GetBool("IsMoving"));
        Debug.Log("Jump " + _anim.GetBool("IsJumping"));
        Debug.Log("Run: " + _anim.GetBool("IsRunning"));

    }

}
