using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductCreator : MonoBehaviour
{
    public event Action<int, int> AmountChanged;

    [SerializeField] private ProductReceiver _receiver;
    [SerializeField] private Transform _reception;
    [SerializeField] private ItemTypes _materialType;
    [SerializeField] private DropItem _product;
    [SerializeField] private Transform[] _places;
    [SerializeField] private List<DropItem> _items;
    [SerializeField, Min(.0f)] private float _timeTakes = .2f;
    [SerializeField, Min(.0f)] private float _timeCreation = 5f;
    [SerializeField, Min(.0f)] private float _timeCooldown = .2f;
    [SerializeField, Min(1)] private int _required = 3;

    private int amount = 0;
    private bool readyTake = true;
    private bool isCreating = false;

    private void Awake() => _receiver.AddedItem += TryTakeItem;
    private void Start() => AmountChanged?.Invoke(amount, _required);

    private void TryTakeItem()
    {
        if (!readyTake || !_receiver.CanGetItem(_materialType)
                       || isCreating)
            return;
        readyTake = false;
        amount++;
        StartCoroutine(Cooldown());

        var item = _receiver.GetItem();
        item.transform.SetParent(null);

        var moveTo = item.GetComponent<MoveTo>();
        moveTo.GotToPlace += () => Destroy(item.gameObject, .1f);
        moveTo.Move(_reception);

        if (amount == _required && _places.Length != _items.Count)
            StartCoroutine(CreateProduct());

        AmountChanged?.Invoke(amount, _required);
    }

    private IEnumerator CreateProduct()
    {
        isCreating = true;
        yield return new WaitForSeconds(_timeCreation);

        var product = Instantiate(_product, _reception.position,
                                            _reception.rotation);

        _items.Add(product);
        var moveTo = product.GetComponent<MoveTo>();
        if (moveTo && !moveTo.IsMoving)
            moveTo.Move(_places[_items.Count]);
        product.PickUp += (_) =>
        {
            if (amount == _required)
                CreateProduct();
            TryTakeItem();
        };

        isCreating = false;
        amount = 0;
        AmountChanged?.Invoke(amount, _required);
        TryTakeItem();
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_timeCooldown);
        readyTake = true;
        TryTakeItem();
    }
}
