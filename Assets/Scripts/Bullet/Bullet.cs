using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SO_BulletData _bulletData;
    private Rigidbody _rb;
    private ShootingEnemy _owner;
    private ObjectPooler _objectPooler;

    public SO_BulletData BulletData => _bulletData;


    public void SetOwner(ShootingEnemy owner) => _owner = owner;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {

        if (_bulletData == null)
        {
            Debug.LogWarning($"BulletData non assegnato su {gameObject.name}");
            return;
        }

        if (_objectPooler == null)
        {
            _objectPooler = ObjectPooler.Instance;
            if (_objectPooler == null)
            {
                Debug.LogWarning("ObjectPooler Instance not found!");
            }
        }

        // Reset
        ResetBullet();
        
        //SelfDestruct
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

        // Verifica che _bulletData sia valido prima di usarlo
        if (_bulletData == null)
        {
            Debug.LogWarning("BulletData null durante ReturnToPool, disattivo l'oggetto");
            gameObject.SetActive(false);
            return;
        }

        // Debug per vedere quale tag stiamo usando
        Debug.Log($"{gameObject.name} has returned to the pool: '{_bulletData.PoolTag}'");

        // Ritorna al pool tramite ObjectPooler
        if (_objectPooler != null)
        {
            _objectPooler.ReturnToPool(gameObject, _bulletData.PoolTag);
        }
        else
        {
            // Fallback se ObjectPooler non è disponibile
            Debug.LogWarning("ObjectPooler non disponibile, uso fallback");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Nome oggetto colpito: {other.gameObject.name}");
        if (other.CompareTag("Player"))
        {
            LifeController lc = other.GetComponentInParent<LifeController>();
            if (lc != null)
            {
                // Controlla che _bulletData sia valido prima di usarlo
                if (_bulletData != null)
                {
                    lc.TakeDamage(_bulletData.Damage);
                    Debug.Log($"Bullet hit player. Damage dealt: {_bulletData.Damage}");
                }
                else
                {
                    Debug.LogWarning("BulletData null, nessun danno inflitto");
                }
            }
            else
            {
                Debug.LogError("LifeController component not found on player.");
            }
        }
        else if (other.CompareTag("Enemy")) return;

            ReturnToPool();
    }


    public void ResetBullet()
    {
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
}