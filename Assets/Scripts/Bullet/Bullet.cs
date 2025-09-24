using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SO_BulletData _bulletData;
    [SerializeField] private Rigidbody _rb;

    private ShootingEnemy _owner;
    private ObjectPooler _objectPooler;

    public SO_BulletData BulletData => _bulletData;


    private void Awake()
    {
        // Ottieni il riferimento all'ObjectPooler
        _objectPooler = ObjectPooler.Instance;

        if (_objectPooler == null)
        {
            Debug.LogError("ObjectPooler Instance not found!");
        }
    }

    private void OnEnable()
    {
        // Reset delle proprietà fisiche quando viene riattivato
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        // Avvia il timer per il ritorno automatico al pool
        Invoke(nameof(ReturnToPool), _bulletData.LifeTime);
    }

    private void OnDisable()
    {
        // Cancella tutti gli Invoke quando viene disabilitato
        CancelInvoke();
    }

    public void ReturnToPool()
    {
        CancelInvoke();

        // Notifica al proprietario (se presente)
        //_owner?.ReturnBulletToPool(this);

        // Ritorna al pool tramite ObjectPooler
        if (_objectPooler != null)
        {
            _objectPooler.ReturnToPool(gameObject, _bulletData.PoolTag);
        }
        else
        {
            // Fallback se ObjectPooler non è disponibile
            gameObject.SetActive(false);
        }

        Debug.Log("Bullet returned to pool.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LifeController lc = other.GetComponentInParent<LifeController>();
            if (lc != null)
            {
                lc.TakeDamage(_bulletData.Damage);
                Debug.Log($"Bullet hit player. Damage dealt: {_bulletData.Damage}");
            }
            else
            {
                Debug.LogError("LifeController component not found on player.");
            }

            ReturnToPool();
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            // Ritorna al pool anche quando colpisce muri o terreno
            Debug.Log($"Bullet hit {other.name}. Returning to pool.");
            ReturnToPool();
        }
    }

    public void SetOwner(ShootingEnemy owner) => _owner = owner;

    // Metodo per resettare il proiettile quando viene recuperato dal pool
    public void ResetBullet()
    {
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
}

