using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payer : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _place;
    [SerializeField] private Coin _banknote;

    public void Pay()
    {
        var b = Instantiate(_banknote, _parent.position,
                                       _parent.rotation);

        b.SetAmount(45);
        var moveTo = b.GetComponent<MoveTo>();
        if (!moveTo.IsMoving)
            moveTo.Move(_place);
    }
}
