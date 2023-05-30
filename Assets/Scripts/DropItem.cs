using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Interactable))]
public class DropItem : MonoBehaviour
{
    public event Action<Item> PickUp;
    [field: SerializeField] public ItemTypes Type { get; private set; }
    [field: SerializeField] public Item HandItem { get; private set; }

    private void Awake() =>
        GetComponent<Interactable>().Enter += (player) =>
        {
            var hand = player.GetComponent<Hand>();
            var moveTo = GetComponent<MoveTo>();

            if (hand.CanAddItem(Type))
            {
                var item = Instantiate(HandItem, transform.position,
                                                 transform.rotation);
                hand.AddInHand(item);
                PickUp?.Invoke(item);
                Destroy(gameObject, .1f);
            }
        };
}
