using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_UI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            Debug.LogError("Attenzione! Qui manca il canvasGroup!");
    }

    private void Start()
    {
        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
    }

    private void Update()
    {
        if (_canvasGroup.alpha >= 1) Time.timeScale = 0;
    }

    public void CallGameOver()
    {
        gameObject.SetActive(true);
        GameObject[] uiElements = GameObject.FindGameObjectsWithTag("UI");
        //foreach (GameObject uiElement in uiElements)
        //{
        //    if (!uiElement.gameObject == this.gameObject)
        //    {
        //        uiElement.gameObject.SetActive(false);                
        //    }
        //    gameObject.SetActive(false); 
        //}
        _canvasGroup.interactable = true;
        StartCoroutine("FadeIn");

       
        
    }
    private IEnumerator FadeIn()
    {
        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }
    public void Quit()
    {
        Debug.Log("I'm changing scene");
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartScene()
    {
        Debug.LogWarning("Player is not alive, cannot reset position.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }


    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.M)) {CallGameOver();}
    //}

}

