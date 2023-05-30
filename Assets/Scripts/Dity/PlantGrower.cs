using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrower : MonoBehaviour
{
    public event Action<GameObject> GrowUp;

    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject[] _stagesGrowth;
    [SerializeField, Min(.0f)] private float _time = 2f;

    private GameObject currStage;

    private void Awake() =>
        GetComponent<MoneyConsumer>().Bought += ()
            => StartCoroutine(nameof(Growth));

    private IEnumerator Growth()
    {
        for (int i = 0; i < _stagesGrowth.Length; i++)
        {
            var newStage = Instantiate(_stagesGrowth[i],
                                       _parent.position,
                                       _parent.rotation);
            Destroy(currStage);
            currStage = newStage;
            if (i == _stagesGrowth.Length - 1)
                GrowUp?.Invoke(currStage);
            yield return new WaitForSeconds(_time);
        }
    }
}
