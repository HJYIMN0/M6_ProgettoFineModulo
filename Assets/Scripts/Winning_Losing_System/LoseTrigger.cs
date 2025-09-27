using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseTrigger : MonoBehaviour
{
    [SerializeField] private int _dmgOnFall;
    private GameObject _player;
    private bool _hasTakenDamage = false;

    private void Start()
    {
        _player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if (_player != null)
        {
            Vector3 playerPos = _player.transform.position;

            if (playerPos.y < transform.position.y && !_hasTakenDamage)
            {
                // Il player è caduto e non ha ancora preso danno
                LifeController playerLifeController = _player.GetComponentInParent<LifeController>();
                if (playerLifeController != null)
                {
                    playerLifeController.TakeDamage(_dmgOnFall);
                }

                // Carica la posizione salvata usando il nuovo sistema
                SaveData data = SaveSystem.Load();
                if (data != null && (data.playerPosX != 0 || data.playerPosY != 0 || data.playerPosZ != 0))
                {
                    SaveSystem.SetGameObjectPosition(_player, data.playerPosX, data.playerPosY, data.playerPosZ);
                    StartCoroutine(ResetDamageFlag());
                }
                else
                {
                    Debug.LogWarning("No valid position saved. Restarting Level");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                _hasTakenDamage = true;
            }
        }
    }

    private IEnumerator ResetDamageFlag()
    {
        // Aspetta 2 frame per assicurarsi che il player sia stato teletrasportato
        yield return null;
        yield return null;
        _hasTakenDamage = false;
    }
}