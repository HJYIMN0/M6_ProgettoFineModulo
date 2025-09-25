    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;
    using System.IO;

    [System.Serializable]
    public class LevelData
    {
        public string levelID;
        public int order;
        public bool isUnlocked;
        public bool isCompleted;

        public LevelData(string id, int orderIndex, bool unlocked = false, bool completed = false)
        {
            levelID = id;
            order = orderIndex;
            isUnlocked = unlocked;
            isCompleted = completed;
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public string username;
        public List<LevelData> levelsProgress;
        public int highScore;
        public int collectedCoins;
        public int totalCoinsCollected;

        // Constructor per inizializzazione di default
        public SaveData()
        {
            levelsProgress = new List<LevelData>();
            // Inizializza con i livelli di default se necessario
            InitializeDefaultLevels();
        }

        public void UpdateCompletedLevels(string levelID)
        {
            if (levelsProgress == null)
                levelsProgress = new List<LevelData>();

            var levelData = levelsProgress.FirstOrDefault(l => l.levelID == levelID);

            if (levelData != null)
            {
                levelData.isCompleted = true;
                // Sblocca il livello successivo
                UnlockNextLevel(levelData.order);
            }
            else
            {
                // Se il livello non esiste, prova a crearlo dal LevelManager
                CreateMissingLevel(levelID);
            }
        }

        private void CreateMissingLevel(string levelID)
        {
            // Qui dovrai integrare con il tuo LevelManager per ottenere i dati del livello
            // Per ora, aggiungiamo una soluzione temporanea
            int newOrder = levelsProgress.Count;
            var newLevel = new LevelData(levelID, newOrder, true, true);
            levelsProgress.Add(newLevel);

            Debug.LogWarning($"Created missing level {levelID} with order {newOrder}. Consider updating your level configuration.");
        }

        private void UnlockNextLevel(int currentLevelOrder)
        {
            var nextLevel = levelsProgress.FirstOrDefault(l => l.order == currentLevelOrder + 1);
            if (nextLevel != null)
            {
                nextLevel.isUnlocked = true;
            }
        }

        public void UpdateUsername(string newUsername)
        {
            username = newUsername;
        }

    // Metodo helper per ottenere il prossimo livello da caricare
    public string GetNextLevelToLoad(LevelConfiguration config)
    {
        if (config == null || config.GetAllLevels() == null)
        {
            Debug.LogWarning("LevelConfiguration non valido.");
            return null;
        }

        foreach (var level in config.GetAllLevels().OrderBy(l => l.order))
        {
            var progress = levelsProgress.FirstOrDefault(p => p.levelID == level.levelID);

            bool isUnlocked = progress?.isUnlocked ?? false;
            bool isCompleted = progress?.isCompleted ?? false;

            // Considera anche il flag da configurazione
            if (level.lvlCompleted)
                isCompleted = true;

            if (isUnlocked && !isCompleted)
            {
                return level.levelID;
            }
        }

        return null;
    }



    // Metodo per inizializzare con livelli configurati esternamente
    public void InitializeLevelsProgress(List<LevelData> allLevels)
        {
            levelsProgress = new List<LevelData>(allLevels);
            // Il primo livello è sempre sbloccato
            if (levelsProgress.Count > 0)
            {
                levelsProgress[0].isUnlocked = true;
            }
        }

        // Inizializzazione di default - puoi personalizzare questo
        private void InitializeDefaultLevels()
        {
            if (levelsProgress == null || levelsProgress.Count == 0)
            {
                // Questi dovrebbero venire da una configurazione esterna
                // Ma per ora li mettiamo qui come fallback
                levelsProgress = new List<LevelData>
                {
                    new LevelData("Level_1", 0, true, false),
                    new LevelData("Level_2", 1, false, false),
                    new LevelData("Level_3", 2, false, false),
                };
            }
        }

        // Helper per contare i livelli completati (per mantenere compatibilità con il SaveSystem)
        public int GetCompletedLevelsCount()
        {
            return levelsProgress?.Count(l => l.isCompleted) ?? 0;
        }
    }

