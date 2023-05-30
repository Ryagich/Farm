using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase : MonoBehaviour
{
    public bool IsOpen = false;
    public MoneyConsumer Consumer { get; private set; }

    public void Init()
    {
        Consumer = GetComponent<MoneyConsumer>();
        Consumer.Bought += () => IsOpen = true;
    }
}
