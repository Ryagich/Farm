using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsTextController : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private void Awake() => CoinManager.ChangeCoins += UpdateText;
    private void UpdateText(int amount) => _text.text = amount.ToString();
}
