using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerParticoleManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerJumpController _jumpController;
    [SerializeField] private LifeController _playerLifeController;
    
    [SerializeField] private ParticleSystem _maxJumpChargeParticles;
    [SerializeField] private ParticleSystem _onFullStopParticles;
    [SerializeField] private ParticleSystem _deathParticles;

    private void Awake()
    {
        if (_playerLifeController == null)
        {
            Debug.LogError("Missing the LifeController Component!");
            return;
        }
        if (_jumpController == null)
        {
            Debug.LogError("Missing The JumpController component!");
            return;
        }
        if (_playerController == null)
        {
            Debug.Log("Missing The PlayerController component!");
            return;
        }

        if (_maxJumpChargeParticles == null || _deathParticles == null || _onFullStopParticles == null)
        {
            Debug.LogError("Missing reference to the particle System!");
        }
    }
    private void Start()
    {
        _jumpController.OnMaxJumpCharge += MaxChargeParticleSystem;
        _playerController.OnFullStop += OnFullStopParticles;
        _playerLifeController._onDeath += OnDeathParticles;
    }

    public void MaxChargeParticleSystem()
    {
        _maxJumpChargeParticles.Play();
    }

    public void OnFullStopParticles()
    {
        _onFullStopParticles.Play();
    }

    public void OnDeathParticles()
    {
        _deathParticles.Play();
    }
}


