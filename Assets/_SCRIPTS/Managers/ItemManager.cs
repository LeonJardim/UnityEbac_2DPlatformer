using Leon.Core.Singleton;
using TMPro;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public SOInt coins;
    [SerializeField] private TMP_Text coinTextHUD;

    private void Start()
    {
        Reset();
    }

    private void Reset()
    {
        coins.Value = 0;
        coinTextHUD.text = "x " + coins.Value.ToString();
    }

    public void AddCoins(int amount = 1)
    {
        coins.Value += amount;
        coinTextHUD.text = "x " + coins.Value.ToString();
    }
}
