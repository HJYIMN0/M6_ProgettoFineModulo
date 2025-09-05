using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStates : MonoBehaviour
{
    [SerializeField] private ResetScene ResetScene;
    [SerializeField] private Transform _newStartPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetScene.SetStartingPos(_newStartPos.transform.position);
        }

    }
}
