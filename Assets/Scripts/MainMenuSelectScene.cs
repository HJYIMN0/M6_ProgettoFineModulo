using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSelectScene : MonoBehaviour
{
    [SerializeField] private GameObject menuScene;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject LelveselectsScene;

    public void ChangeScene()
    {
        menuScene.SetActive(!menuScene.activeSelf);
        LelveselectsScene.SetActive(!LelveselectsScene.activeSelf);

        instructions.SetActive(menuScene.activeSelf);
    }
}
