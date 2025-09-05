using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _hDistance = 5f; 
    [SerializeField] private float _vDistance = 2f;
    [SerializeField] private float _rotationSpeed = 250f;    

    private float _currentAngle = 0f;

    private void Awake()
    {
        if (_target == null)
        {
            Debug.LogError("Target not set for CameraOrbit script.");
            _target = GameObject.FindGameObjectWithTag("Player")?.transform;
            Debug.Log("Target is set as Player.");
        }
    }

    private void Update()
    {
        if (_target == null) return;

        float horizontalInput = Input.GetAxis("Mouse X");

        if (horizontalInput != 0)
        {
            _currentAngle += horizontalInput * _rotationSpeed * Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        // Calcola la nuova posizione della camera in base all'angolo
        Quaternion rotation = Quaternion.Euler(0, _currentAngle, 0);
        Vector3 _offset = rotation * new Vector3(0, _vDistance, -_hDistance);

        transform.position = _target.position + _offset;
        transform.LookAt(_target.position);

        // Allinea la forward del target a quella della camera, solo sull’asse orizzontale
        Vector3 cameraForward = transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        _target.forward = cameraForward;
    }
}

