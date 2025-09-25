using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningTrigger : MonoBehaviour
{
    [SerializeField] private WinningUI _winningUI;
    private bool _hasCollectedAllCoins = false;

    public void Awake()
    {
        if (_winningUI == null)
        {
            _winningUI = GetComponent<WinningUI>();
            if (_winningUI == null)
            {
                Debug.Log("Manca il WinningUI!");
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_hasCollectedAllCoins)
            {
                Debug.Log("Non hai raccolto tutte le monete!");
                return;
            }


            string currentSceneName = SceneManager.GetActiveScene().name;
            LevelManager.Instance.OnLevelCompleted(currentSceneName);

            _winningUI.OnUICalled();

            Debug.Log($"Livello '{currentSceneName}' completato e salvato.");
        }
    }

    public void SetHasCollectedAllCoins(bool value)
    {
        if (value != _hasCollectedAllCoins)
            _hasCollectedAllCoins = value;
    }
}
