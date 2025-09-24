using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "ScriptableObjects/Bullets")]
public class SO_BulletData : ScriptableObject
{

        [Header("Movement")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _lifeTime = 5f;

        [Header("Combat")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private bool _piercing = false;
        [SerializeField] private int _maxPierceTargets = 1;

        [Header("Visual Effects")]
        [SerializeField] private GameObject _hitEffect;
        [SerializeField] private GameObject _muzzleFlash;
        [SerializeField] private TrailRenderer _trailPrefab;

        [Header("Audio")]
        [SerializeField] private AudioClip _shootSound;
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private float _volume = 1f;

        [Header("Pooling")]
        [SerializeField] private string _poolTag = "Bullet";

        // Public getters
        public float Speed => _speed;
        public float LifeTime => _lifeTime;
        public int Damage => _damage;
        public bool Piercing => _piercing;
        public int MaxPierceTargets => _maxPierceTargets;
        public GameObject HitEffect => _hitEffect;
        public GameObject MuzzleFlash => _muzzleFlash;
        public TrailRenderer TrailPrefab => _trailPrefab;
        public AudioClip ShootSound => _shootSound;
        public AudioClip HitSound => _hitSound;
        public float Volume => _volume;
        public string PoolTag => _poolTag;
    
}
