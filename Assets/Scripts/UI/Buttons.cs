using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Buttons : MonoBehaviour
{

    [SerializeField] private string mainMenuName = "MainMenu";
    [SerializeField] private LevelConfiguration levelConfiguration;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    public void StartLevel()
    {
        Debug.Log("Clicked Start!");

        SaveData data = SaveSystem.Load();

        if (data == null || data.levelsProgress == null || data.levelsProgress.Count == 0)
        {
            Debug.LogWarning("Nessun dato di salvataggio trovato o lista livelli vuota.");
            return;
        }

        // Trova il livello sbloccato con order più alto
        LevelData lastUnlocked = null;

        foreach (var level in data.levelsProgress)
        {
            if (level.isUnlocked)
            {
                if (lastUnlocked == null || level.order > lastUnlocked.order)
                {
                    lastUnlocked = level;
                }
            }
        }

        // Fallback: se nessun livello è sbloccato, carica il primo
        if (lastUnlocked == null)
        {
            lastUnlocked = data.levelsProgress.FirstOrDefault(l => l.order == 0);
            Debug.Log("Nessun livello sbloccato trovato, carico il primo disponibile.");
        }

        if (lastUnlocked != null)
        {
            Debug.Log($"Caricamento del livello: {lastUnlocked.levelID}");
            LevelManager.Instance.LoadLevel(lastUnlocked.levelID);
        }
        else
        {
            Debug.Log("Nessun livello disponibile da caricare.");
        }
    }

    public void NextLevel()
    {
        string nextLevelID = SaveSystem.GetNextLevelToLoad(levelConfiguration);

        if (!string.IsNullOrEmpty(nextLevelID))
        {
            LevelManager.Instance.LoadLevel(nextLevelID);
        }
        else
        {
            Debug.Log("Tutti i livelli sono stati completati o nessun livello disponibile.");
        }
    }

}
