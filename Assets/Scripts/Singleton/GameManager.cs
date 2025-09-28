using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : AbstractSingleton<GameManager>
{
    public TimeManager TimeManager { get; private set; }
    public GameObject Player { get; private set; }
    public CoinManager CoinManager { get; private set; }
    public Timer Timer { get; private set; }
    public SaveData SaveData { get; private set; }

    public WinningTrigger WinningTrigger { get; private set; }

    public override bool IsDestroyedOnLoad() => true;
    public override bool ShouldDetatchFromParent() => true;


    public void OnEnable()
    {

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
            SaveSystem.Load();
        }

        WinningTrigger = FindObjectOfType<WinningTrigger>();
        if (WinningTrigger == null)
        {
            Debug.LogError("WinningTrigger not found in the scene.");
        }

        Timer = FindObjectOfType<Timer>();
        if (Timer == null)
        {
            Debug.LogError("Timer not found in scene");
        }
        // Carica i dati salvati
        SaveData data = SaveSystem.Load();
        if (data != null && (data.playerPosX != 0 || data.playerPosY != 0 || data.playerPosZ != 0))
        {
            // Applica posizione salvata al player
            SaveSystem.SetGameObjectPosition(Player, data.playerPosX, data.playerPosY, data.playerPosZ);

            // Applica HP salvati al player
            LifeController lifeController = Player.GetComponentInParent<LifeController>();
            lifeController.SetHp(data.hp);
            lifeController.SetMaxHp(data.maxHp);
        }
    }
}