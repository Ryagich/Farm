using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InterfaceHider))]
[RequireComponent(typeof(MoneyConsumer))]
public class TomatManager : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<PlantGrower>().GrowUp += (tomatPlantObj) =>
        {
            var tomatPlant = tomatPlantObj.GetComponent<TomatPlant>();
            tomatPlant.SetHider(GetComponent<InterfaceHider>());
            tomatPlant.Wilted += ()
                => GetComponent<MoneyConsumer>().Init();
        };
    }
}
