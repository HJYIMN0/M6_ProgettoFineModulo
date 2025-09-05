using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int _hp = 5;
    [SerializeField] private int _maxHp = 5;
    [SerializeField] private bool _maxHpOnStart = true;

    public UnityEvent<int, int> OnLifeChanged; //hp, max hp
    public UnityEvent _onDeath;

    public int GetHp() => _hp;
    public int GetMaxHp() => _maxHp;
    public void SetHp(int hp)
    {
        _hp = Mathf.Clamp(hp, 0, _maxHp);
    }

    public void SetMaxHp(int maxHp)
    {
        _maxHp = Mathf.Max(maxHp, 1);
        if (_hp > _maxHp)
        {
            _hp = _maxHp;
        }
    }

    private void Start()
    {
        if (_maxHpOnStart)
        {
            SetMaxHp(_maxHp);
            SetHp(_maxHp);
        }
    }

    public void TakeDamage(int damage)
    {
        SetHp(_hp - damage);
        Debug.Log($"Damage taken: {damage}. Current HP: {_hp}/{_maxHp}");
        OnLifeChanged?.Invoke(_hp, _maxHp);
        if (_hp <= 0)
        {
            Die(gameObject);
        }
    }

    public void Heal(int amount)
    {
        SetHp(_hp + amount);
    }

    public void Die(GameObject target)
    {
        if (_hp > 0) return;
        else
        {
            if (target.CompareTag("Player"))
            {
                Debug.Log("Player has died.");
                _hp = 0;
                Destroy(target);
                _onDeath?.Invoke();
                
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Da modificare per caricare la scena di Game Over o simile
            }
            else
            {
                Debug.Log($"{target.name} has died.");
                Destroy(target);
            }
        }
    }

}
