using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class ProductReceiver : MonoBehaviour
{
    public event Action AddedItem;

    [SerializeField] private ItemTypes Type;
    [SerializeField] private Transform[] _places;
    [SerializeField] private List<Item> _items = new();
    [SerializeField, Min(.0f)] private float _time = .2f;

    private int maxItems;
    private bool isBuyer = false;

    private void Awake()
    {
        var interactable = GetComponent<Interactable>();
        interactable.Enter += (player) =>
            StartCoroutine(TrySetItem(player.GetComponent<Hand>()));
        interactable.Exit += (_) => StopAllCoroutines();
    }

    public void Init(ItemTypes type, int max, bool isBuyer) //нужно для машин и покупателей
    {
        Type = type;
        maxItems = max;
        this.isBuyer = isBuyer;
    }

    private IEnumerator TrySetItem(Hand hand)
    {
        while (hand.CanGiveItem(Type)
            && ((!isBuyer && _places.Length != _items.Count)
            || _items.Count < maxItems))
        {
            var item = hand.RemoveItem();
            var index = _items.Count;
            item.transform.SetParent(_places[index]);
            item.GetComponent<MoveTo>().Move(_places[index]);
            _items.Add(item);
            AddedItem?.Invoke();
            yield return new WaitForSeconds(_time);
        }
    }

    public bool CanGetItem(ItemTypes type)
        => type == Type && _items.Count > 0;

    public Item GetItem()
    {
        var item = _items.Last();
        _items.Remove(item);
        return item;
    }
}
