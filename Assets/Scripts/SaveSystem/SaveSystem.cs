using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem 
{
    public static string GetPath() => Application.persistentDataPath + "/save.data";

    public static bool Save(SaveData data)
    {
        string path = GetPath();
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(path, json);

        Debug.Log($"Saved {json} to {path}");

        return true;
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

        string jsonString = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(jsonString);

        Debug.Log($"Loaded {jsonString} from {path}");

        return data;
    }

    public static bool SaveFinishedLevel(string levelID)
    {
        SaveData data = Load() ?? new SaveData();
        data.UpdateCompletedLevels(levelID);
        Debug.Log($"Level {levelID} completed. Total completed levels: {data.levelsCompleted.Count}");
        return Save(data);
    }

}
