using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableMesh : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is here!");
            collision.transform.root.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has left!");
            collision.transform.root.SetParent(null);
        }
    }
}
