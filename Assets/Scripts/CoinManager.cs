using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static event Action<int> ChangeCoins;
    public static CoinManager Instance;
    private int coins = 0;

    private void Awake() => Instance = this;

    public bool TrySetCoins(int amount)
    {
        if (coins + amount < 0)
            return false;
        coins += amount;
        ChangeCoins?.Invoke(coins);
        return true;
    }
}
