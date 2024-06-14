using System;
using System.Collections;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public event Action<Rabbit> WantedEat;
    public event Action<int,int> AmountChanged;
    public bool  WaitFood = false;

    [field: SerializeField] public ItemTypes Type { get; private set; } = ItemTypes.Carrot;
    [SerializeField, Min(1)] private int _required = 3;
    [SerializeField, Min(.0f)] private float _timeDigest = 2f;
    [SerializeField, Min(.0f)] private float _timeCreation = 5f;
    [SerializeField, Min(.0f)] private float _timeChill = 5f;
    [SerializeField] private DropItem Egg;

    private int amount = 0;
    private bool isDigest = false;
    private bool isCreating = false;
    private bool isChill = false;

    private void Start()
    {
        AmountChanged?.Invoke(amount, _required);
        WantedEat?.Invoke(this);
    }

    public bool CanTakeItem()
        => !isDigest && !isCreating && !isChill && !WaitFood
        && amount < _required;

    public void EatUp()
    {
        isDigest = true;
        amount++;
        AmountChanged?.Invoke(amount, _required);
        StartCoroutine(Digest());
        if (amount == _required)
            StartCoroutine(CreateProduct());
    }

    private IEnumerator CreateProduct()
    {
        isCreating = true;
        yield return new WaitForSeconds(_timeCreation);
        Instantiate(Egg, transform.position, transform.rotation);
        isCreating = false;
        amount = 0;
        AmountChanged?.Invoke(amount,_required);
        StartCoroutine(Chill());
    }

    private IEnumerator Chill()
    {
        isChill = true;
        yield return new WaitForSeconds(_timeChill);
        isChill= false;
        WantedEat?.Invoke(this);
    }

    private IEnumerator Digest()
    {
        yield return new WaitForSeconds(_timeDigest);
        isDigest = false;
        WantedEat?.Invoke(this);
    }
}
