using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_MonoBehaviour : MonoBehaviour
{
    public string levelID { get; private set; }
    private void Start()
    {
        levelID = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
    private void Update()
    {
        //per testare soltanto

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }

        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        SaveSystem.Save(data);
    }

    public void LoadGame() 
    {
        SaveData saveData = SaveSystem.Load();
    }
}
