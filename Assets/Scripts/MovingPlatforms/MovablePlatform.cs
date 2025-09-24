using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour, iMovable
{
    [Header("Time Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waitTime = 5f;
    [Header("Meshes settings")]
    [SerializeField] private Transform movableMesh;
    [SerializeField] private Transform[] desiredPos;

    private int _posIndex;
    private float _waitTimer = 0f;
    private bool _isWaiting;
    

    private void Start()
    {
        if (desiredPos == null || desiredPos.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name} can't find the positions!");
            return;
        }

        if (movableMesh == null)
        {
            Debug.LogError($"You forgot to assign the Movable Mesh!");
        }
    }

    private void Update()
    {
        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitTime)
            {
                _isWaiting = false;
                _waitTimer = 0f;

                _posIndex = (_posIndex + 1) % desiredPos.Length;
            }
        }
        else
        {
            // mi sto muovendo verso il target
            Transform target = desiredPos[_posIndex];
            Move(movableMesh.transform.position, target.position, speed);

            if (Vector3.Distance(movableMesh.transform.position, target.position) < 0.1f)
            {
                // appena raggiunto, inizio la fase di attesa
                _isWaiting = true;
            }
        }
    }
    public void Move(Vector3 currentPos, Vector3 targetPos, float speed)
    {
        movableMesh.transform.position = Vector3.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
    }
}
