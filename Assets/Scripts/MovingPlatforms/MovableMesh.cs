using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableMesh : MonoBehaviour
{
    private Transform playerSceneTransform;

    private void Start()
    {
        // Lo so che è deprecabile usare il nome
        //Nella scena 00, avevo lasciato una logica molto più semplice
        //Di collisionEnter/Exit
        //Per qualche motivo, nella scena 01, non funziona
        //penso dipenda dal fatto che è child di un prefab con molti altri children
        //Avrei voluto trovare una soluzione migliore, ma per limiti di tempo, questa è l'unica che ho trovato.
        GameObject playerSceneObj = GameObject.Find("PlayerScene");
        if (playerSceneObj != null)
        {
            playerSceneTransform = playerSceneObj.transform;
        }
        else
        {
            Debug.LogError("GameObject 'PlayerScene' not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform playerTransform = FindPlayerInHierarchy(other.transform);

        if (playerTransform != null)
        {
            Debug.Log("Player is here!");
            playerTransform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform playerTransform = FindPlayerInHierarchy(other.transform);

        if (playerTransform != null && playerSceneTransform != null)
        {
            Debug.Log("Player has left!");
            playerTransform.SetParent(playerSceneTransform);
        }
    }

    private Transform FindPlayerInHierarchy(Transform startTransform)
    {
        // Controlla se questo è il GameObject "Player"
        if (startTransform.name == "Player")
            return startTransform;

        // Risale nella gerarchia cercando "Player"
        Transform current = startTransform;
        while (current.parent != null)
        {
            current = current.parent;
            if (current.name == "Player")
                return current;
        }

        return null; // Non trovato
    }
}