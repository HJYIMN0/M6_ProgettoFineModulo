using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class LevelSelector : MonoBehaviour
{
    public void SelectLevel00() => SceneManager.LoadScene("lvl00");

    public void SelectLevel01() => SceneManager.LoadScene("lvl01");

}
