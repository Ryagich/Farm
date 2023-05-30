using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerAtCamera : MonoBehaviour
{
    private void Awake() => transform.LookAt(Camera.main.transform);
}
