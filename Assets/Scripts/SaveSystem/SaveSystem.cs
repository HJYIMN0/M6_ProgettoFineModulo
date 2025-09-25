using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    public static string GetPath() => Application.persistentDataPath + "/save.data";

    public static bool Save(SaveData data)
    {
        try
        {
            string path = GetPath();
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            Debug.Log($"Saved to {path}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save: {e.Message}");
            return false;
        }
    }

    public static bool DoesSaveExist()
    {
        return File.Exists(GetPath());
    }

    public static SaveData Load()
    {
        string path = GetPath();
        if (!DoesSaveExist())
        {
            Debug.LogWarning($"Save file not found in {path}");
            return null;
        }

        try
        {
            string jsonString = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonString);
            Debug.Log($"Loaded from {path}");
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load save: {e.Message}");
            return null;
        }
    }

    public static bool SaveFinishedLevel(string levelID)
    {
        SaveData data = Load() ?? new SaveData();
        data.UpdateCompletedLevels(levelID);

        int completedCount = data.GetCompletedLevelsCount();
        Debug.Log($"Level {levelID} completed. Total completed levels: {completedCount}");

        return Save(data);
    }

    // Nuovo metodo per ottenere il prossimo livello da caricare
    public static string GetNextLevelToLoad(LevelConfiguration config)
    {
        SaveData data = Load() ?? new SaveData();
        return data.GetNextLevelToLoad(config);
    }
}
