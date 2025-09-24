using UnityEngine;

public class ShootingEnemy : AbstractEnemy
{
    private float _shootTimer = 0f;
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private float _shootInterval = 1f;
    [SerializeField] private SO_BulletData _bulletData;

    private string _bulletPoolTag;
    
    private ObjectPooler _pooler;

    private void Start()
    {
        _pooler = ObjectPooler.Instance;
        if (_pooler == null)
            Debug.LogWarning($"{gameObject.name}has no reference to PoolInstance!");

        if (_bulletData == null)
        {
            Debug.LogWarning("Missing the Bullet Scriptable Object reference!");
            return;
        }
        _bulletPoolTag = _bulletData.PoolTag;
    }


    private void Update()
    {
        TryShoot();
        Rotate();
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
        if (IsPlayerInRange(out GameObject player))
        {
            if (player != null)
            {
                Debug.Log("Player detected within range. Shooting...");

                // Ottieni proiettile dall'ObjectPooler
                GameObject bulletObj = _pooler.SpawnFromPool(_bulletPoolTag, _spawnLocation.position, Quaternion.identity);
                if (bulletObj != null)
                {
                    Bullet bullet = bulletObj.GetComponent<Bullet>();
                    Rigidbody bulletRb = bulletObj.GetComponent<Rigidbody>();
                    if (bulletRb == null)
                    {
                        Debug.LogError("Bullet Rigidbody is null. Cannot shoot.");
                        return;
                    }
                    bullet.SetOwner(this);
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    bulletRb.velocity = Vector3.zero;
                    bulletRb.angularVelocity = Vector3.zero;
                    bulletRb.AddForce(direction * bullet.BulletData.Speed, ForceMode.Impulse);
                    Debug.Log("Bullet shot with speed: " + bullet.BulletData.Speed);
                }
            }
        }
        else
        {
            Debug.Log("No player detected within range. Not shooting.");
        }
    }

    public void Rotate()
    {
        if (IsPlayerInRange(out GameObject player))
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f; // blocca l'asse verticale

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyData.RotationSpeed * Time.deltaTime);
            }
        }
    }
}