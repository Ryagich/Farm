using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatPlant : MonoBehaviour
{
    public event Action Wilted;

    [SerializeField] private List<Transform> tomatPlaces = new();
    [SerializeField] private DropItem _tomatPref;
    [SerializeField, Min(.0f)] private float _time = 5f;

    [SerializeField] private InterfaceHider hider;
    private List<DropItem> tomats = new();

    private void Awake() => StartCoroutine(GrowthTomats());

    private IEnumerator GrowthTomats()
    {
        yield return new WaitForSeconds(_time);
        foreach (var place in tomatPlaces)
        {
            var newTomat = Instantiate(_tomatPref,
                                        place.position,
                                        place.rotation);
            tomats.Add(newTomat);
            newTomat.PickUp += (_) =>
            {
                tomats.Remove(newTomat);
                CheckTomats();
            };
            yield return new WaitForSeconds(_time);
        }
        StartCoroutine(Wilt());
    }

    private IEnumerator Wilt()
    {
        while (tomats.Count > 0)
            yield return new WaitForSeconds(_time);
        Wilted?.Invoke();
        Destroy(gameObject);
    }

    public void SetHider(InterfaceHider hider) => this.hider = hider;

    private void CheckTomats() 
        => hider.ChangeSelectedState(tomats.Count > 0);
}
