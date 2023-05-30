using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private void Awake()
    {
        var consumer = GetComponent<MoneyConsumer>();
        consumer.ChangeCost += UpdateText;
        UpdateText(consumer.Cost);
    }
    private void UpdateText(int amount) => _text.text = amount.ToString();
}