using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform NextItemPlace;
    [field: SerializeField] public ItemTypes Type { get; private set; }
}
