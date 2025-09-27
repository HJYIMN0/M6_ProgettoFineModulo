using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    [Header("Timer settings")]
    [SerializeField] private float _timerIncreaseValue = 20f;

    [Header("Scale Settings")]
    [SerializeField] private float _amplitude;
    [SerializeField] private float _speed;

    [Header("References")]
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
                
                lifeController.SetHp(lifeController.GetMaxHp());
                //Non riuscivo a controllare da qui la posizione, ho creato una nuova classe
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

            GameManager.Instance.Timer.IncreaseTimerByValue(_timerIncreaseValue);
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