using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : AbstractSingleton<LevelManager>
{
    [Header("Lista delle scene di gioco")]
    [SerializeField] private string[] scenes;
    
    [Header("Player GameObject")]
    [SerializeField] private GameObject player;

    public string[] Scenes => scenes;
    public override bool IsDestroyedOnLoad() => false;
    public override bool ShouldDetatchFromParent() => true;

    private void Start()
    {
        LoadPlayerPosition();
        InitializeLevelProgress();
    }

    private void InitializeLevelProgress()
    {
        SaveData data = SaveSystem.Load();
        if (data == null || data.levelsProgress == null || data.levelsProgress.Count == 0)
        {
            data = new SaveData();
            data.levelsProgress = new List<SaveData.LevelData>();

            for (int i = 0; i < scenes.Length; i++)
            {
                data.levelsProgress.Add(new SaveData.LevelData(scenes[i], i)
                {
                    isUnlocked = (i == 0)
                });
            }

            SaveSystem.Save(data);
            Debug.Log("Progressi dei livelli inizializzati.");
        }
    }

    public void LoadLevel(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name nullo o vuoto.");
            return;
        }

        // Salva la posizione corrente del _player prima di cambiare livello
        SavePlayerPosition();

        Debug.Log($"Caricamento scena: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    public void SaveCompletedLevel(string sceneName)
    {
        SaveData data = SaveSystem.LoadOrInitialize();
        var level = data.levelsProgress.Find(l => l.levelID == sceneName);
        if (level == null)
        {
            Debug.LogWarning($"Il livello {sceneName} non è stato trovato nei progressi.");
            return;
        }

        // Marca il livello come completato
        level.isCompleted = true;
        Debug.Log($"Livello completato: {sceneName}");

        // Sblocca il prossimo livello se esiste
        int nextOrder = level.order + 1;
        var nextLevel = data.levelsProgress.Find(l => l.order == nextOrder);
        if (nextLevel != null && !nextLevel.isUnlocked)
        {
            nextLevel.isUnlocked = true;
            Debug.Log($"Sbloccato il livello successivo: {nextLevel.levelID}");
        }

        SaveSystem.Save(data);
    }

    // Salva la posizione corrente del _player
    public void SavePlayerPosition()
    {
        if (player == null)
        {
            Debug.LogWarning("Player GameObject non assegnato nell'inspector.");
            return;
        }

        SaveData data = SaveSystem.LoadOrInitialize();
        Vector3 pos = SaveSystem.GetGameObjectPosition(player);
        SaveSystem.Vector3ToFloats(pos, out data.playerPosX, out data.playerPosY, out data.playerPosZ);

        SaveSystem.Save(data);
        Debug.Log($"Posizione player salvata: {pos}");
    }

    // Carica e applica la posizione salvata al _player
    public void LoadPlayerPosition()
    {
        if (player == null)
        {
            Debug.LogWarning("Player GameObject non assegnato nell'inspector.");
            return;
        }

        SaveData data = SaveSystem.Load();
        if (data == null)
        {
            Debug.Log("Nessun dato di salvataggio trovato, il player mantiene la posizione di default.");
            return;
        }

        if (new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ) != Vector3.zero)
        {
            SaveSystem.SetGameObjectPosition(player, data.playerPosX, data.playerPosY, data.playerPosZ);
            Debug.Log($"Posizione player caricata: ({data.playerPosX}, {data.playerPosY}, {data.playerPosZ})");
        }

    }
}