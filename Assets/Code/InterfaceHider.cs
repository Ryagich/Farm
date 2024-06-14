using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceHider : MonoBehaviour
{
    [SerializeField] private GameObject _banner;
    [SerializeField] private GameObject _selected;

    public void ChangeBannerState(bool state)
        => _banner.SetActive(state);
    public void ChangeSelectedState(bool state)
        => _selected.SetActive(state);
}
