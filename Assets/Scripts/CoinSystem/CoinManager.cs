using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int totalCoins { get; private set; }
    public int collectedCoins { get; private set; } = 0;   
    
    private GameManager gameManager => GameManager.Instance;
    private SaveData saveData => gameManager.SaveData;
    private WinningTrigger winningTrigger => gameManager.WinningTrigger;

    public void Start()
    {

        GameObject[] coinObjects = GameObject.FindGameObjectsWithTag("Coin");
        if (coinObjects == null || coinObjects.Length == 0)
        {
            Debug.LogWarning("No coins found in the scene with the 'Coin' tag.");
            return;
        }
        //set total coins at start
        totalCoins = coinObjects.Length;
        Debug.Log("Total Coins in Scene: " + totalCoins);
        
        if (winningTrigger == null)
        {
            Debug.LogError($"{gameObject.name} can't find WinningTrigger in the scene.");
        }

        //reset collected coins for each individual level
        collectedCoins = 0;
        if (saveData != null)
        {
            saveData.collectedCoins = collectedCoins;
        }
        else
        {
            Debug.Log("No save data found, starting fresh.");
            collectedCoins = 0;
            SaveSystem.Save(new SaveData());
        }
    }

    public void IncreaseCollectedCoinsByValue(int value)
    {
        collectedCoins += value;
        Debug.Log("Collected Coins: " + collectedCoins);
        if (collectedCoins >= totalCoins)
        {
            collectedCoins = totalCoins; // Ensure it doesn't exceed totalCoins
            Debug.Log("All coins collected!");
            winningTrigger.SetHasCollectedAllCoins(true);
            // Trigger any event or action for collecting all coins
            collectedCoins = 0; // Reset collected coins for potential replay
            saveData.totalCoinsCollected += totalCoins;
        }
        saveData.collectedCoins = collectedCoins;
        SaveSystem.Save(saveData);
    }
}
