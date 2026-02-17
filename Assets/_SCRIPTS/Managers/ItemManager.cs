using Leon.Core.Singleton;
using TMPro;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public int coins;
    [SerializeField] private TMP_Text coinTextHUD;

    private void Start()
    {
        Reset();
    }

    private void Reset()
    {
        coins = 0;
    }

    public void AddCoins(int amount = 1)
    {
        coins += amount;
        coinTextHUD.text = "x " + coins;
    }
}
