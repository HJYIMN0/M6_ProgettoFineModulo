using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string username;
    public List<float> highScores;
    public int playerCoins;
    public int totalCollectedCoins;

    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    public int hp = 5;
    public int maxHp = 5;
    public string currentScene = "";

    [System.Serializable]
    public class LevelData
    {
        public string levelID;
        public int order;
        public bool isUnlocked;
        public bool isCompleted;

        public LevelData(string id, int order)
        {
            levelID = id;
            this.order = order;
            isUnlocked = false;
            isCompleted = false;
        }
    }

    public List<LevelData> levelsProgress = new List<LevelData>();
}