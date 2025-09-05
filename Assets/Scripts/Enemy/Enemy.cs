using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{

    public Queue<Bullet> _bulletsQueue = new Queue<Bullet>();
    private float _shootTimer = 0f; // Timer for shooting interval
    [SerializeField] protected Bullet _bulletPreFab { get; private set; } // Prefab for the bullet
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] protected float _gizmoRadius = 10;
    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] private float _shootInterval = 1f; // Time between shots



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _gizmoRadius);
    }

    private void Update()
    {
        TryShoot();
    }

    public void TryShoot()
    {
        _shootTimer += Time.deltaTime; 

        if (_shootTimer >= _shootInterval)
        {
            Debug.Log("Timer reached shoot interval: " + _shootInterval);
            Shoot(); 
            _shootTimer = 0f; 
        }
    }

    public virtual void Shoot()
    {
        if (Physics.OverlapSphere(transform.position, _gizmoRadius, _playerLayer).Length != 0)
        {
            GameObject player = Physics.OverlapSphere(transform.position, _gizmoRadius, _playerLayer)[0].gameObject;
            Debug.Log("Player detected within range. Shooting...");
            Bullet bullet = GetBullet();
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb == null)
            {
                Debug.LogError("Bullet Rigidbody is null. Cannot shoot.");
                return;
            }
            else 
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                bulletRb.AddForce(direction * bullet.GetSpeed(), ForceMode.Impulse);
                bulletRb.angularVelocity = Vector3.zero;
                Debug.Log("Bullet shot with speed: " + bullet.GetSpeed());
            }
        }
        else Debug.Log("No player detected within range. Not shooting.");
    }

    public virtual Bullet GetBullet()
    {
        if (_bulletsQueue.Count > 0)
        {
            Bullet bullet = _bulletsQueue.Dequeue();
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero; // Reset velocity
            bullet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // Reset angular velocity
            bullet.transform.position = _spawnLocation.position;
            bullet.gameObject.SetActive(true);
            Debug.Log("Bullet retrieved from queue. Remaining bullets in queue: " + _bulletsQueue.Count);
            return bullet;
        }
        else
        {
            Debug.Log("No bullets available in queue. Instantiating a new bullet.");
            Bullet bullet = Instantiate(_bulletPreFab, _spawnLocation.position, Quaternion.identity);
            bullet.SetOwner(this); // Set the owner of the bullet
            bullet.transform.position = _spawnLocation.position;
            return bullet;
        }
    }

    public void ReturnBulletToQueue(Bullet bullet)
    {
        bullet.gameObject.SetActive(false); // Deactivate the bullet
        _bulletsQueue.Enqueue(bullet); // Add the bullet back to the queue
        Debug.Log("Bullet returned to queue. Total bullets in queue: " + _bulletsQueue.Count);
    }
}
