using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public ItemTypes Type { get; private set; }

    [SerializeField] private ParticleSystem particleS;
    [SerializeField] private GameObject _bannerC;
    [SerializeField] private GameObject _bannerT;
    [SerializeField] private GameObject _bannerTP;
    [SerializeField] private GameObject _bannerE;
    [SerializeField] private GameObject _bannerM;
    [SerializeField] private GameObject _selected;

    private ProductReceiver receiver;
    private int required; 
    private int amount;
    private Animator animC;
    private GameObject banner;
    private Payer payer;

    private void Awake()
    {
        receiver = GetComponent<ProductReceiver>();
        receiver.AddedItem += OnAddedItem;
        animC = GetComponent<Animator>();
        payer = GetComponent<Payer>();
    }

    public void GetOrder()
    {
        Type = (ItemTypes)Random.Range(1, 5);
        required = Random.Range(1, 5);
        amount = 0;
        receiver.Init(Type, required, true);

        switch (Type)
        {
            case ItemTypes.Carrot:
                banner = _bannerC;
                break;
            case ItemTypes.Tomat:
                banner = _bannerT;
                break;
            case ItemTypes.k:
                banner = _bannerTP;
                break;
            case ItemTypes.Egg:
                banner = _bannerE;
                break;
            case ItemTypes.m:
                banner = _bannerM;
                break;
        }
    }

    public void ClearReceiver()
    {
        while (receiver.CanGetItem(Type))
        {
            var item = receiver.GetItem();
            if (!item)
                return;
            Destroy(item.gameObject);
        }
    }

    public void ChangeInterfaceState(bool isActive)
    {
        banner.SetActive(isActive);
        _selected.SetActive(isActive);
    }

    public void OpenInterface()
    {
        banner.SetActive(true);
        _selected.SetActive(true);
        particleS.enableEmission = false;
    }

    private void OnAddedItem()
    {
        amount++;
        if (required == amount)
        {
            animC.SetTrigger("IsClose");
            ChangeInterfaceState(false);
            particleS.enableEmission = true;
            payer.Pay();
        }
    }
}
