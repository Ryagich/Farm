using TMPro;
using UnityEngine;

[RequireComponent(typeof(ProductCreator))]
public class CreatorInterface : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void Awake()
        => GetComponent<ProductCreator>().AmountChanged += (a, m)
        => _text.text = a.ToString() + " / " + m.ToString();
}
