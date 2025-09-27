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

        Debug.Log($"Scene disponibili: {LevelManager.Instance.Scenes.Length}");
        foreach (var name in LevelManager.Instance.Scenes)
        {
            Debug.Log($"Scena: {name}");
        }

        return data;
    }

    // Prende il Vector3 della position di un GameObject
    public static Vector3 GetGameObjectPosition(GameObject gameObject)
    {
        return gameObject.transform.position;
    }

    // Trasforma un Vector3 in 3 float separati
    public static void Vector3ToFloats(Vector3 position, out float x, out float y, out float z)
    {
        x = position.x;
        y = position.y;
        z = position.z;
    }

    // Applica 3 float al Vector3 position di un GameObject
    public static void SetGameObjectPosition(GameObject gameObject, float x, float y, float z)
    {
        gameObject.transform.position = new Vector3(x, y, z);
    }
}