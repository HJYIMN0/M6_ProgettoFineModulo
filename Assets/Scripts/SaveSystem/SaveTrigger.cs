using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    [SerializeField] private float _amplitude;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _graphics;
    [SerializeField] private Transform newPos; // Posizione da salvare

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            LifeController lifeController = other.GetComponentInParent<LifeController>();
            if (lifeController == null)
                lifeController = other.GetComponent<LifeController>();

            if (lifeController != null)
            {
                // Ripristina HP
                lifeController.SetHp(lifeController.GetMaxHp());

                // Trova il PlayerSpawn component e aggiorna la spawn position
                SpawnPositionHandler playerSpawn = other.GetComponentInParent<SpawnPositionHandler>();

                if (playerSpawn != null)
                {
                    playerSpawn.SetSpawnPosition(newPos.position);
                    Debug.Log($"Checkpoint saved! New spawn position: {newPos.position}");
                }
                else
                {
                    Debug.LogError("PlayerSpawn component not found on player!");
                }

                // Se vuoi ancora salvare le monete o altro nel SaveSystem, puoi farlo qui
                CoinManager coinCollector = GameManager.Instance.CoinManager;
                if (coinCollector != null)
                {
                    SaveData currentData = SaveSystem.LoadOrInitialize();
                    currentData.totalCollectedCoins = coinCollector.CollectedCoins;
                    SaveSystem.Save(currentData);
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Can't find LifeController on " + other.gameObject.name);
            }
        }
    }

    private void Update()
    {
        float t = Time.time * _speed;
        float scaleX = _graphics.transform.localScale.x + Mathf.Cos(t) * _amplitude;
        float scaleY = _graphics.transform.localScale.y + Mathf.Sin(t) * _amplitude;
        float scaleZ = _graphics.transform.localScale.z + Mathf.Cos(t + Mathf.PI / 2f) * _amplitude;
        _graphics.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }
}