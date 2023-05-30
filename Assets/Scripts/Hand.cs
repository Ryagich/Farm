using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class Hand : MonoBehaviour
{
    [field: SerializeField] public Transform HandTrans { get; private set; }
    [SerializeField, Min(0)] private int _maxItems = 3;

    [SerializeField] private List<Item> items = new(); // debug SerializeField
    private ItemTypes CurrItemType = ItemTypes.None;
    private Transform itemPlace;

    private void Awake() => itemPlace = HandTrans;

    public bool CanAddItem(ItemTypes type)
        => (CurrItemType == ItemTypes.None
         || CurrItemType == type && items.Count < _maxItems);

    public void AddInHand(Item item)
    {
        items.Add(item);
        CurrItemType = item.Type;

        item.transform.SetParent(itemPlace);
        item.GetComponent<MoveTo>().Move(itemPlace);
        itemPlace = item.NextItemPlace;
    }

    public bool CanGiveItem(ItemTypes type) => CurrItemType == type;

    public Item RemoveItem()
    {
        var item = items.Last();
        item.transform.SetParent(null);
        items.Remove(item);

        if (items.Count > 0)
            itemPlace = items.Last().NextItemPlace;
        else
        {
            itemPlace = HandTrans;
            CurrItemType = ItemTypes.None;
        }

        return item;
    }
}

public enum ItemTypes
{
    None = 0,
    Carrot = 1,
    Tomat = 2,
    Egg = 3,
    m,
    k
}
