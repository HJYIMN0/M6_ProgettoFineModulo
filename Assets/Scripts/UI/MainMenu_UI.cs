using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private CanvasGroup _mainMenuCanva;
    [SerializeField] private CanvasGroup _levelSelector;

    private void Start()
    {
        if (_mainMenuCanva == null || _levelSelector == null)
        {
            Debug.LogError("Attenzione, devi assegnare dall'inspector main menu o level selector!");
            return;
        }
        else 
        {
            _mainMenuCanva.gameObject.SetActive(true);
            _levelSelector.gameObject.SetActive(false);
            _mainMenuCanva.alpha = 1;
            _levelSelector.alpha = 0;
            _mainMenuCanva.interactable = true;
            _levelSelector.interactable = false;
        }
    }


    public void LoadScene()
    {
        SceneManager.LoadScene("MainLevel");        
    }


    public void Quit()
    {
        Debug.Log("Purtroppo non è possiobile uscire dal gioco");
    }


    public void ChangeScene()
    {
        if (_mainMenuCanva != null && _levelSelector != null)
        {
            if (_mainMenuCanva.alpha == 0)
            {
                _mainMenuCanva.gameObject.SetActive(true);
                _levelSelector.gameObject.SetActive(false);
                _levelSelector.alpha = 0;
                _levelSelector.interactable = false;
                _mainMenuCanva.alpha = 1;
                _mainMenuCanva.interactable = true;
            }
            else if (_mainMenuCanva.alpha == 1)
            {
                _mainMenuCanva.gameObject.SetActive(false);
                _levelSelector.gameObject.SetActive(true);
                _levelSelector.alpha = 1;
                _levelSelector.interactable = true;
                _mainMenuCanva.alpha = 0;
                _mainMenuCanva.interactable= false;
            }
        }
    }
}
