using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    [field: SerializeField] List<Purchase> Purchases = new();

    private int index = 0;

    private void Awake()
    {
        foreach (var purshase in Purchases)
        {
            purshase.Init();
            purshase.Consumer.Bought += UpdateState;
            purshase.gameObject.SetActive(purshase.IsOpen);
        }
    }

    private void UpdateState()
    {
        if (Purchases.Count > index + 1)
        {
            Purchases[index].Consumer.Bought-= UpdateState;
            index++;
            Purchases[index].gameObject.SetActive(true);
        }
    }
}
