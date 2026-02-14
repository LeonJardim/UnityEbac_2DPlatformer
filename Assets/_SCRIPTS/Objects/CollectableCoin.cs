using UnityEngine;

public class CollectableCoin : Collectable
{
    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddCoins();
    }
}
