using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectedCoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;

    private void Start()
    {
        GameManager.Instance.CoinManager.OnCoinCollected += DisplayCoin;
        DisplayCoin(GameManager.Instance.CoinManager.CollectedCoins, GameManager.Instance.CoinManager.TotalCoins);
    }

    public void CallDisplayCoin()
    {
        DisplayCoin(GameManager.Instance.CoinManager.CollectedCoins, GameManager.Instance.CoinManager.TotalCoins);
    }


    public void DisplayCoin(int collectedcoins, int totalCoins)
    {
        if (GameManager.Instance.WinningTrigger.CollectedAllCoins)
        {
            _coinText.text = $"{GameManager.Instance.CoinManager.TotalCoins} / {GameManager.Instance.CoinManager.TotalCoins}";
            return;
        }
        _coinText.text = $"{collectedcoins} / {totalCoins}";
    }

    private void OnDestroy()
    {
        if (GameManager.Instance.CoinManager != null)
        {
            GameManager.Instance.CoinManager.OnCoinCollected -= DisplayCoin;
        }
    }

    public void ShowText()
    {
        //Debug.Log("Called!");
        _coinText.text = "First you must collect all coins!";
    }

}
