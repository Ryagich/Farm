using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RabbitText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _banner, _rabbit;

    private void Awake()
    {
        GetComponent<Rabbit>().AmountChanged += (a, m)
           => _text.text = a.ToString() + " / " + m.ToString();
        _banner.parent = null;
    }
           

    private void FixedUpdate() => _banner.position = _rabbit.position;
}
