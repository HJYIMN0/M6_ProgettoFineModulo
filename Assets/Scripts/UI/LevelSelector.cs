using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class LevelSelector : MonoBehaviour
{
    public void DisplayNoLevel() => Debug.Log("Future levels incoming!");

    public void SelectLevel1() => SceneManager.LoadScene("MainLevel");
}
