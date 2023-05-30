using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private GameObject _stage1;
    [SerializeField] private GameObject _stage2;

    private void Awake()
       => GetComponent<MoneyConsumer>().Bought += () =>
       {
            _stage1.SetActive(false);
            _stage2.SetActive(true);
       };
}
