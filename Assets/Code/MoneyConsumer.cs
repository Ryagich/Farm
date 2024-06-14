using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MoneyConsumer : MonoBehaviour
{
    public event Action Bought;
    public event Action<int> ChangeCost;

    [field: SerializeField, Min(0)] public int Cost { get; private set; } = 5;

    [SerializeField, Min(.0f)] private float _time = .15f;
    [SerializeField] private GameObject _dropCoin;
    [SerializeField] private Transform _transform;

    private int amount;
    private int coinsLeft;
    private Interactable interactable;
    private InterfaceController interfaceController;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.Enter += (player) => StartCoroutine(TakeCoinCor(player));
        interactable.Exit += (_) => StopAllCoroutines();
        interfaceController = GetComponent<InterfaceController>();

        Init();
    }

    public void Init()
    {
        amount = Cost;
        coinsLeft = Cost;
        if (interfaceController)
            interfaceController.Activate();
        ChangeCost?.Invoke(amount);
    }

    private IEnumerator TakeCoinCor(GameObject player)
    {
        while (amount > 0)
        {
            if (!CoinManager.Instance.TrySetCoins(-1))
                break;
            amount--;
            ChangeCost?.Invoke(amount);
            var dropCoin = Instantiate(_dropCoin, player.transform.position,
                                                  player.transform.rotation);
            var moveTo = dropCoin.GetComponent<MoveTo>();
            moveTo.GotToPlace += TakeCoin;
            moveTo.GotToPlace += () => Destroy(dropCoin);
            moveTo.Move(_transform);
            yield return new WaitForSeconds(_time);
        }
    }

    private void TakeCoin()
    {
        coinsLeft--;
        if (coinsLeft == 0)
            Bought?.Invoke();
    }
}
