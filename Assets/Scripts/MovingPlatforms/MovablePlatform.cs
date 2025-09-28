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

    [Header("Player Detection")]
    [SerializeField] private bool requiresPlayer = true; // Se true, aspetta il player per iniziare

    private int _posIndex;
    private float _waitTimer = 0f;
    private bool _isWaiting;
    private bool _hasStartedMoving = false;

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
        // Se richiede il player e non ha ancora iniziato, non fare nulla
        if (requiresPlayer && !_hasStartedMoving)
        {
            return;
        }

        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitTime)
            {
                _isWaiting = false;
                _waitTimer = 0f;
                _posIndex = (_posIndex + 1) % desiredPos.Length; // Correzione: rimosso *
            }
        }
        else
        {
            // Mi sto muovendo verso il target
            Transform target = desiredPos[_posIndex];
            Move(movableMesh.transform.position, target.position, speed);
            if (Vector3.Distance(movableMesh.transform.position, target.position) < 0.1f)
            {
                // Appena raggiunto, inizio la fase di attesa
                _isWaiting = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hasStartedMoving)
        {
            Debug.Log("Player detected! Platform starting movement...");
            _hasStartedMoving = true;
            _isWaiting = false; // Prima volta: inizia subito a muoversi
            _waitTimer = 0f;
        }
    }

    // Rimosso OnTriggerExit - non serve più

    public void Move(Vector3 currentPos, Vector3 targetPos, float speed)
    {
        movableMesh.transform.position = Vector3.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
    }

    // Metodo per resettare la piattaforma (opzionale)
    public void ResetPlatform()
    {
        _posIndex = 0;
        _waitTimer = 0f;
        _isWaiting = false;
        _hasStartedMoving = false;

        if (desiredPos.Length > 0)
        {
            movableMesh.transform.position = desiredPos[0].position;
        }
    }
}