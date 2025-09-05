using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] public UnityEvent<int> _onCoinCollected;
    //[SerializeField] private PlayerController _player;
    [SerializeField] private float _setActiveDelay = 2f;
    public static int score { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerController>().SetSecondJump(true);
            other.GetComponentInParent<PlayerController>()._onSecondJump?.Invoke(true);
            score += value;
            _onCoinCollected?.Invoke(score);
            gameObject.SetActive(false);
            Debug.Log(score);
        }
    }

    private void OnDisable()
    {
        Invoke("SetActiveTrue", _setActiveDelay);
        value = 0;
    }

    public void SetActiveTrue()
    {
        gameObject.SetActive(true);
    }

    //private void Update()
    //{
    //    Debug.Log(value);
    //    Debug.Log($"score uguale a {score}");
    //}
}
