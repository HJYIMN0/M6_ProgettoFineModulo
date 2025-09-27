using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "ScriptableObjects/Bullets")]
public class SO_BulletData : ScriptableObject
{

    [Header("Movement")]
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _heightDifference = 5f;

    [Header("Combat")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private bool _piercing = false;
    [SerializeField] private int _maxPierceTargets = 1;

    [Header("Pooling")]
    [SerializeField] private string _poolTag = "Bullet";

    // Public getters
    public float Speed => _speed;
    public float LifeTime => _lifeTime;

    public float HeightDifference => _heightDifference;
    public int Damage => _damage;
    public bool Piercing => _piercing;
    public int MaxPierceTargets => _maxPierceTargets;
    public string PoolTag => _poolTag;
    
}
