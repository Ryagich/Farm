using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public event Action<GameObject> Enter;
    public event Action<GameObject> Exit;

    private void OnTriggerEnter(Collider other)
        => Enter?.Invoke(other.gameObject);
    private void OnTriggerExit(Collider other)
        => Exit?.Invoke(other.gameObject);
}
