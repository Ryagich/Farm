using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoverToPlayer : MonoBehaviour
{
    private void Awake() =>
        GetComponentInChildren<Interactable>().Enter += (GameObject player) =>
        {
            GetComponent<MoveTo>().Move(player.GetComponent<Hand>().HandTrans);
            if (TryGetComponent<Collider>(out var collider))
                collider.enabled = false;
        };
}
