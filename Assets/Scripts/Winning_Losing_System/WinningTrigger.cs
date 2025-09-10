using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningTrigger : MonoBehaviour
{
    [SerializeField] private WinningUI _winningUI;

    public void Awake()
    {        
        if (_winningUI == null)
        {
            _winningUI = GetComponent<WinningUI>();
            if (_winningUI == null)
            {
                Debug.Log("Manca il WinningUI!");
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //aggiungere la logica delle monete raccolte necessarie 
            //if (playerCoins >= requiredCoins)...
            _winningUI.OnUICalled();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _winningUI.OnUICalled();
        }
    }
}
