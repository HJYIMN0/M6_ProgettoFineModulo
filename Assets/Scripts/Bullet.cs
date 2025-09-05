using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f; // Speed of the bullet
    [SerializeField] private int _damage = 1; // Damage dealt by the bullet
    [SerializeField] private float _lifeTime = 5f; // Time before the bullet is destroyed
    [SerializeField] private Rigidbody _rb; // Rigidbody component for physics interactions

    private Enemy _owner;

    public float GetSpeed() => _speed;

    private void OnEnable()
    {
        Invoke("ReturnToPool", _lifeTime);
    }



    public void ReturnToPool()
    {
        CancelInvoke();
        gameObject.SetActive(false);
        _owner?.ReturnBulletToQueue(this);
        Debug.Log("Bullet returned to pool.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LifeController _lc = other.GetComponentInParent<LifeController>();
            if (_lc != null)
            {
                _lc.TakeDamage(_damage);
                Debug.Log($"Bullet hit player. Damage dealt: {_damage}");
            }
            else if (_lc == null)         
                Debug.LogError("LifeController component not found on player.");            
        }
        else
            Debug.Log($"Bullet hit {other.name}. No damage dealt.");
        ReturnToPool();
        
    }

    public void SetOwner(Enemy owner) => _owner = owner;
}

