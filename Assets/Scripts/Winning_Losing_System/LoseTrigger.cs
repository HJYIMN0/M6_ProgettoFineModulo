using UnityEngine;

public class LoseTrigger : MonoBehaviour
{
    [SerializeField] private int _dmgOnFall = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePlayerFall(other.gameObject);
        }
    }

    private void HandlePlayerFall(GameObject player)
    {
        // Trova il LifeController e applica il danno
        LifeController playerLifeController = player.GetComponentInParent<LifeController>();

        if (playerLifeController != null)
        {
            int currentHp = playerLifeController.GetHp();
            int newHp = currentHp - _dmgOnFall;

            if (newHp > 0)
            {
                // Il player sopravvive, applica danno e respawn
                playerLifeController.TakeDamage(_dmgOnFall);

                // Trova il componente PlayerSpawn e respawn alla sua spawnPos (già impostata dal SaveTrigger)
                SpawnPositionHandler playerSpawn = player.GetComponentInParent<SpawnPositionHandler>();
                if (playerSpawn != null)
                {
                    playerSpawn.RespawnAtCheckpoint();
                    Debug.Log($"Player fell! HP: {currentHp} -> {newHp}. Respawned at checkpoint.");
                }
                else
                {
                    Debug.LogError("PlayerSpawn component not found on player!");
                }
            }
            else
            {
                // Il player muore, applica solo il danno e lascia che il LifeController gestisca
                playerLifeController.TakeDamage(_dmgOnFall);
                Debug.Log("Player fell and died!");
            }
        }
        else
        {
            Debug.LogError("LifeController not found on player!");
        }
    }
}