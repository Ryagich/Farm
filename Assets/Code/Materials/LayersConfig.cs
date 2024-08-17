using UnityEngine;

namespace Code.Materials
{
    [CreateAssetMenu(fileName = "LayersConfig", menuName = "LayersConfig")]

    public class LayersConfig : ScriptableObject
    {
        [field: SerializeField] public LayerMask Extension_Layer { get; private set; }
    }
}