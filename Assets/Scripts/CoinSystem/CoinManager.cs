using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int totalCoins { get; private set; }
    public int collectedCoins { get; private set; } = 0;
    private WinningTrigger winningTrigger;
    //public int CollectedCoins
    //{
    //    get { return collectedCoins; }
    //    set
    //    {
    //        collectedCoins = value;
    //        if (collectedCoins >= totalCoins)
    //        {
    //            Debug.Log("All coins collected!");
    //            // Trigger any event or action for collecting all coins
    //        }
    //    }
    //}

    private void Awake()
    {
        GameObject[] coinObjects = GameObject.FindGameObjectsWithTag("Coin");
        if (coinObjects == null || coinObjects.Length == 0)
        {
            Debug.LogWarning("No coins found in the scene with the 'Coin' tag.");
            return;
        }
        totalCoins = coinObjects.Length;

        winningTrigger = FindObjectOfType<WinningTrigger>();
        if (winningTrigger == null)
        {
            Debug.LogError($"{gameObject.name} can't find WinningTrigger in the scene.");
        }
    }

    public void IncreaseCollectedCoinsByValue(int value)
    {
        collectedCoins += value;
        if (collectedCoins >= totalCoins)
        {
            collectedCoins = totalCoins; // Ensure it doesn't exceed totalCoins
            Debug.Log("All coins collected!");
            winningTrigger.SetHasCollectedAllCoins(true);
            // Trigger any event or action for collecting all coins
        }
    }
}
