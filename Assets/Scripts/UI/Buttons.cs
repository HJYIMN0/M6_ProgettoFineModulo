using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] private string mainMenuName = "MainMenu";

    public void MenuQuit()
    {
        Debug.Log("Non posso ancora uscire dal gioco!");
    }

    public void Quit()
    {
        Debug.Log("Clicked Quit!");
        SceneManager.LoadScene(mainMenuName);
    }

    public void Retry()
    {
        Debug.Log("Clicked Retry!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartLevel()
    {
        Debug.Log("Clicked Start!");
        LoadFirstIncompleteLevel();
    }

    public void NextLevel()
    {
        Debug.Log("Clicked Next!");
        LoadFirstIncompleteLevel();
    }

    /// <summary>
    /// Carica il primo livello non completato.
    /// Se tutti i livelli sono completati, torna al MainMenu.
    /// </summary>
    private void LoadFirstIncompleteLevel()
    {
        SaveData data = SaveSystem.LoadOrInitialize();

        bool found = false;

        foreach (var level in data.levelsProgress)
        {
            if (!level.isCompleted)
            {
                Debug.Log($"Caricamento del livello: {level.levelID}");
                LevelManager.Instance.LoadLevel(level.levelID);
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.Log("Hai completato tutti i livelli");
            SceneManager.LoadScene(mainMenuName);
        }
    }
}