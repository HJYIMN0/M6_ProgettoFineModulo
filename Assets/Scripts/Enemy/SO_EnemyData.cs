using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy")]
public class SO_EnemyData : ScriptableObject
{

    [SerializeField] private float _gizmoRadius = 10;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private CheckerShape _gizmoShape = CheckerShape.Sphere;
    
    public float GizmoRadius => _gizmoRadius;
    public float RotationSpeed => _rotationSpeed;
    public LayerMask PlayerLayer => _playerLayer;
    public CheckerShape GizmoShape => _gizmoShape;
}

