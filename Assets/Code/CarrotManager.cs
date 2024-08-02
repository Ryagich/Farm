using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotManager : MonoBehaviour
{
    private void Awake()
        => GetComponent<PlantGrower>().GrowUp += RestartGrowth;

    private void RestartGrowth(GameObject carrot)
    {
        if (!carrot)
        {
            GetComponent<MoneyConsumer>().Init();
            return;
        }
        carrot.GetComponent<DropItem>().PickUp += (item) =>
               GetComponent<MoneyConsumer>().Init();
    }
}
