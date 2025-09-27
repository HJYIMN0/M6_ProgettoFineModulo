using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionHandler : MonoBehaviour
{
    [SerializeField] private Vector3 _spawnPos;

    public Vector3 SpawnPos => _spawnPos;

    void Start()
    {
        // Imposta la spawn iniziale alla posizione corrente se non è già impostata
        if (_spawnPos == Vector3.zero)
            _spawnPos = transform.position;
    }

    public void SetSpawnPosition(Vector3 newSpawnPos)
    {
        _spawnPos = newSpawnPos;
    }

    public void RespawnAtCheckpoint()
    {
        // Ferma il movimento
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Teletrasporta
        transform.position = _spawnPos;
    }
}
