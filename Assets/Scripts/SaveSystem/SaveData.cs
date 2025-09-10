using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string username;
    public List<string> levelsCompleted;
    public int highScore;

    public void UpdateCompletedLevels(string levelID)
    {
        if (levelsCompleted == null)
            levelsCompleted = new List<string>();
        if (!levelsCompleted.Contains(levelID))
            levelsCompleted.Add(levelID);
    }

    public void UpdateUsername(string newUsername)
    {
        username = newUsername;
    }
}
