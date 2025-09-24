using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Buttons : MonoBehaviour
{

    [SerializeField] private string mainMenuName = "MainMenu";

    public void MenuQuit()
    {
        Debug.Log("Non posso ancora uscire dal gioco!");
    }

    public void Quit()
    {
        SceneManager.LoadScene(mainMenuName);       
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    public void Start()
    {
        SaveData data = SaveSystem.Load();
        
    }
}
