using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : AbstractSingleton<GameManager>
{
    public override bool IsDestroyedOnLoad() => true;
    public override bool ShouldDetatchFromParent() => true;
    public GameObject Player { get; private set; }
    public CoinManager CoinManager { get; private set; }

    public SaveData SaveData { get; private set; }

    public WinningTrigger WinningTrigger { get; private set; }

    public override void Awake()
    {
        base.Awake();


        Debug.Log("GameManager started.");

        Player = GameObject.FindWithTag("Player");
        if (Player == null)
        {
            Debug.LogError("Player object not found in the scene. Make sure it is tagged as 'Player'.");
        }

        CoinManager = FindObjectOfType<CoinManager>();
        if (CoinManager == null)
        {
            Debug.LogError("CoinManager not found in the scene.");
        }

        SaveData = SaveSystem.Load();
        if (SaveData == null)
        {
            Debug.Log("No save data found, starting fresh.");
            SaveData = new SaveData();
            SaveSystem.Save(SaveData);
        }

        WinningTrigger = FindObjectOfType<WinningTrigger>();
        if (WinningTrigger == null)
        {
            Debug.LogError("WinningTrigger not found in the scene.");
        }
    }

}
