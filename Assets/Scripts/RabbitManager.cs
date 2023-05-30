using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitManager : MonoBehaviour
{
    [SerializeField] private ItemTypes _type = ItemTypes.Carrot;
    [SerializeField] private MoneyConsumer[] _consumers;
    [SerializeField] private List<RabbitMovement> _rabbits;
    [SerializeField] private BoxCollider _area;
    [SerializeField] private GameObject _rabbit;
    [SerializeField] private Transform _parent;
    [SerializeField] private ProductReceiver _receiver;

    private void Awake()
    {
        foreach (var consumer in _consumers)
        {
            consumer.Bought += () =>
            {
                var pos = consumer.gameObject.transform.position;
                var rabbit = Instantiate(_rabbit, new(pos.x, .9f, pos.z),
                                                  consumer.gameObject.transform.rotation);
                var rm = rabbit.GetComponent<RabbitMovement>();
                _rabbits.Add(rm);
                rm.Init(_area);
                rm.FindNextPlace();
                rm.MoveToTarget();

                var r = rm.GetComponent<Rabbit>();
                r.WantedEat += TryGetFood;

                Destroy(consumer.gameObject);
            };
        }

        _receiver.AddedItem += TryFeed;
    }

    private void TryFeed()
    {
        foreach (var rm in _rabbits)
        {
            var rabbit = rm.GetComponent<Rabbit>();
            if (!_receiver.CanGetItem(_type))
                return;
            if (!rabbit.CanTakeItem())
                continue;
            var item = _receiver.GetItem();
            var moveTo = item.GetComponent<MoveTo>();
            rabbit.WaitFood = true;
            moveTo.GotToPlace += () =>
            {
                rabbit.EatUp();
                rabbit.WaitFood = false;
                Destroy(moveTo.gameObject);
            };
            moveTo.Move(rabbit.transform);
        }
    }

    private void TryGetFood(Rabbit rabbit)
    {
        if (!_receiver.CanGetItem(_type))
            return;
        if (!rabbit.CanTakeItem())
            return;
        var item = _receiver.GetItem();
        var moveTo = item.GetComponent<MoveTo>();
        rabbit.WaitFood = true;
        moveTo.GotToPlace += () =>
        {
            rabbit.EatUp();
            rabbit.WaitFood = false;
            Destroy(moveTo.gameObject);
        };
        moveTo.Move(rabbit.transform);
    }
}
