using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpykedFloor : AbstractEnemy
{
    [SerializeField] private float _restTimer = 5f;
    [SerializeField] private float _timeBeforeNextMove = 2f;
    private Animator _animator;
    private bool _isCoroutineRunning;
    private bool _canDealDamage = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
            Debug.LogError($"Missing Animator on {gameObject.name}");
    }
    private void Update()
    {
        if (IsPlayerInRange(out GameObject player))
        {
            if (player != null)
            {
                StartCoroutine(DealDamage());
            }
        }        
    }

    public IEnumerator DealDamage()
    {
        if (_isCoroutineRunning) yield break;

        _isCoroutineRunning = true;
        _animator.SetBool("isPlayerColliding", true);

        //  Attendi 1 secondo prima di infliggere danno
        yield return new WaitForSeconds(1f);
        _canDealDamage = true; //  Attiva il flag per infliggere danno


        yield return new WaitForSeconds(_timeBeforeNextMove - 1f);
        _animator.SetBool("isPlayerColliding", false);
        _canDealDamage = false;

        //  Tempo di riposo
        yield return new WaitForSeconds(_restTimer);

        _isCoroutineRunning = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Found!");
            if (!_isCoroutineRunning) return;
            if (!_canDealDamage) return;

            LifeController lc = collision.gameObject.GetComponentInChildren<LifeController>();
            {
                if (lc != null) 
                {
                    Debug.Log("Applying Damage!");
                    lc.TakeDamage(1);
                }
                else Debug.Log("Missing LifeController on " + collision.gameObject.name);
            }
        }
    }

}
