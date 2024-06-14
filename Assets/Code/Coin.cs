using System;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField, Min(0)] private int _amount = 1;

    private void Awake() => GetComponent<MoveTo>().GotToPlace += () =>
    {
        CoinManager.Instance.TrySetCoins(_amount);
        Destroy(gameObject);
    };

    public void SetAmount(int amount) => _amount = amount;
}
