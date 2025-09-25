using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// ScriptableObject per configurare i livelli nell'editor
[CreateAssetMenu(fileName = "new Level", menuName = "ScriptableObjects/Levels")]
public class LevelConfiguration : ScriptableObject
{
    [System.Serializable]
    public class LevelInfo
    {
        public string levelID;
        public string displayName;
        public string sceneName;
        public int order;
        public Sprite levelIcon;
        [TextArea(2, 4)]
        public string description;

        [Header("Debug")]
        public bool lvlCompleted = false; // nuovo campo

        public LevelInfo(string id, string name, string scene, int orderIndex)
        {
            levelID = id;
            displayName = name;
            sceneName = scene;
            order = orderIndex;
        }
    }

    [SerializeField] private List<LevelInfo> levels = new List<LevelInfo>();

    public List<LevelInfo> GetAllLevels() => levels;

    public LevelInfo GetLevelByID(string levelID)
    {
        return levels.FirstOrDefault(l => l.levelID == levelID);
    }

    public LevelInfo GetLevelByOrder(int order)
    {
        return levels.FirstOrDefault(l => l.order == order);
    }

    [ContextMenu("Auto-Generate Level IDs")]
    private void AutoGenerateLevelIDs()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (string.IsNullOrEmpty(levels[i].levelID))
            {
                levels[i].levelID = $"Level_{i + 1:00}";
            }
            levels[i].order = i;
        }
    }
}

// Manager principale per gestire i livelli
public class LevelManager : AbstractSingleton<LevelManager>
{
    [Header("Configuration")]
    [SerializeField] private LevelConfiguration levelConfiguration;

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private SaveData currentSaveData;
    public override bool IsDestroyedOnLoad() => true;

    public override bool ShouldDetatchFromParent() => true;



    public override void Awake()
    {
        base.Awake(); // Chiama la logica del singleton

        InitializeLevelSystem(); // Avvia la logica dei livelli
    }


    private void InitializeLevelSystem()
    {
        if (levelConfiguration == null)
        {
            Debug.LogError("LevelConfiguration not assigned to LevelManager!");
            return;
        }

        // Carica o crea i dati di salvataggio
        currentSaveData = SaveSystem.Load() ?? new SaveData();

        // Sincronizza i livelli dalla configurazione
        SyncLevelsWithConfiguration();

        // Salva i dati aggiornati
        SaveSystem.Save(currentSaveData);

        if (debugMode)
        {
            LogLevelStatus();
        }
    }

    private void SyncLevelsWithConfiguration()
    {
        var configLevels = levelConfiguration.GetAllLevels();
        List<LevelData> syncedLevels = new List<LevelData>();

        foreach (var configLevel in configLevels.OrderBy(l => l.order))
        {
            var existingLevel = currentSaveData.levelsProgress?.FirstOrDefault(l => l.levelID == configLevel.levelID);

            if (existingLevel != null)
            {
                existingLevel.order = configLevel.order;

                //  sincronizza completamento da configurazione
                if (configLevel.lvlCompleted)
                {
                    existingLevel.isCompleted = true;
                    currentSaveData.UpdateCompletedLevels(configLevel.levelID); //  sblocca il successivo
                }

                syncedLevels.Add(existingLevel);
            }
            else
            {
                bool isUnlocked = configLevel.order == 0;
                bool isCompleted = configLevel.lvlCompleted;

                syncedLevels.Add(new LevelData(configLevel.levelID, configLevel.order, isUnlocked, isCompleted));
            }
        }

        currentSaveData.levelsProgress = syncedLevels;
    }


    // Metodi pubblici per interagire con i livelli
    public void CompleteLevel(string levelID)
    {
        var levelData = currentSaveData.levelsProgress?.FirstOrDefault(l => l.levelID == levelID);
        if (levelData != null && !levelData.isCompleted)
        {
            currentSaveData.UpdateCompletedLevels(levelID);
            SaveSystem.Save(currentSaveData);

            if (debugMode)
            {
                Debug.Log($"Level {levelID} completed and saved!");
            }
        }
    }

    public void LoadLevel(string levelID)
    {
        var configLevel = levelConfiguration.GetLevelByID(levelID);
        var saveLevel = currentSaveData.levelsProgress?.FirstOrDefault(l => l.levelID == levelID);

        if (configLevel == null)
        {
            Debug.LogError($"Level {levelID} not found in configuration!");
            return;
        }

        if (saveLevel != null && !saveLevel.isUnlocked)
        {
            Debug.LogWarning($"Level {levelID} is locked!");
            return;
        }

        if (debugMode)
        {
            Debug.Log($"Loading level: {configLevel.displayName} (Scene: {configLevel.sceneName})");
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(configLevel.sceneName);
    }

    public void LoadNextLevel()
    {
        string nextLevelID = SaveSystem.GetNextLevelToLoad(levelConfiguration);

        if (!string.IsNullOrEmpty(nextLevelID))
        {
            LoadLevel(nextLevelID);
        }
        else
        {
            Debug.Log("No next level available or all levels completed!");
        }
    }

    public bool IsLevelUnlocked(string levelID)
    {
        var levelData = currentSaveData.levelsProgress?.FirstOrDefault(l => l.levelID == levelID);
        return levelData?.isUnlocked ?? false;
    }

    public bool IsLevelCompleted(string levelID)
    {
        var levelData = currentSaveData.levelsProgress?.FirstOrDefault(l => l.levelID == levelID);
        var configLevel = levelConfiguration.GetLevelByID(levelID);

        return (levelData?.isCompleted ?? false) || (configLevel?.lvlCompleted ?? false);
    }

    public LevelConfiguration.LevelInfo GetLevelInfo(string levelID)
    {
        return levelConfiguration.GetLevelByID(levelID);
    }

    public List<LevelConfiguration.LevelInfo> GetUnlockedLevels()
    {
        var unlockedLevelIDs = currentSaveData.levelsProgress?
            .Where(l => l.isUnlocked)
            .Select(l => l.levelID)
            .ToList() ?? new List<string>();

        return levelConfiguration.GetAllLevels()
            .Where(l => unlockedLevelIDs.Contains(l.levelID))
            .OrderBy(l => l.order)
            .ToList();
    }

    public int GetTotalLevelsCount()
    {
        return levelConfiguration.GetAllLevels().Count;
    }

    public int GetCompletedLevelsCount()
    {
        return currentSaveData.GetCompletedLevelsCount();
    }

    // Metodi di utilità per debugging
    [ContextMenu("Unlock All Levels")]
    private void UnlockAllLevels()
    {
        if (currentSaveData?.levelsProgress != null)
        {
            foreach (var level in currentSaveData.levelsProgress)
            {
                level.isUnlocked = true;
            }
            SaveSystem.Save(currentSaveData);
            Debug.Log("All levels unlocked!");
        }
    }

    [ContextMenu("Reset All Progress")]
    private void ResetAllProgress()
    {
        if (currentSaveData?.levelsProgress != null)
        {
            foreach (var level in currentSaveData.levelsProgress)
            {
                level.isCompleted = false;
                level.isUnlocked = false;
            }
        }

        // Sblocca solo il livello con order == 0 dalla configurazione
        var firstConfigLevel = levelConfiguration.GetAllLevels().FirstOrDefault(l => l.order == 0);
        if (firstConfigLevel != null)
        {
            var saveLevel = currentSaveData.levelsProgress.FirstOrDefault(l => l.levelID == firstConfigLevel.levelID);
            if (saveLevel != null)
            {
                saveLevel.isUnlocked = true;
                Debug.Log($"Sbloccato: {saveLevel.levelID} (order: {saveLevel.order})");
            }
        }

        SaveSystem.Save(currentSaveData);
        LogLevelStatus();

        foreach (var level in currentSaveData.levelsProgress)
        {
            Debug.Log($"[DEBUG] {level.levelID} - Unlocked: {level.isUnlocked}, Completed: {level.isCompleted}, Order: {level.order}");
        }

        Debug.Log(Application.persistentDataPath);
    }

    private void LogLevelStatus()
    {
        if (currentSaveData?.levelsProgress == null) return;

        Debug.Log("=== LEVEL STATUS ===");
        foreach (var level in currentSaveData.levelsProgress.OrderBy(l => l.order))
        {
            var configLevel = levelConfiguration.GetLevelByID(level.levelID);
            string status = level.isCompleted ? "COMPLETED" : (level.isUnlocked ? "UNLOCKED" : "LOCKED");
            Debug.Log($"{level.order + 1}. {configLevel?.displayName ?? level.levelID} - {status}");
        }
        Debug.Log($"Progress: {GetCompletedLevelsCount()}/{GetTotalLevelsCount()}");
    }

    public LevelConfiguration.LevelInfo GetLevelInfoByOrder(int order)
    {
        return levelConfiguration.GetLevelByOrder(order);
    }

    // Chiamato da altri script quando un livello viene completato
    public void OnLevelCompleted(string levelID)
    {
        CompleteLevel(levelID);

        // Puoi aggiungere qui logiche aggiuntive come:
        // - Mostrare UI di completamento
        // - Dare ricompense
        // - Aggiornare statistiche
        // - etc.
    }
}