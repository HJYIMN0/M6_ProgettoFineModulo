using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveSystem
{
    public static string GetPath() => Application.persistentDataPath + "/save.data";

    public static bool Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath(), json);
        Debug.Log($"Saved {json} to {GetPath()}");
        return true;
    }

    public static bool DoesSaveExist() => File.Exists(GetPath());

    public static SaveData Load()
    {
        if (!DoesSaveExist()) return null;

        string json = File.ReadAllText(GetPath());
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log($"Loaded {json} from {GetPath()}");
        return data;
    }

    public static SaveData LoadOrInitialize()
    {
        SaveData data = Load();

        if (data == null)
        {
            data = new SaveData();
            data.levelsProgress = new List<SaveData.LevelData>();

            var sceneIDs = LevelManager.Instance.Scenes;
            for (int i = 0; i < sceneIDs.Length; i++)
            {
                var level = new SaveData.LevelData(sceneIDs[i], i);
                level.isUnlocked = (i == 0); // Solo il primo sbloccato
                data.levelsProgress.Add(level);
            }

            Save(data);
            Debug.Log("SaveData inizializzato da LevelManager.");
        }

        return data;
    }
}