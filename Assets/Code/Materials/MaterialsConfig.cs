using UnityEngine;

namespace Code.Materials
{
    [CreateAssetMenu(fileName = "MaterialsConfig", menuName = "MaterialsConfig")]

    public class MaterialsConfig : ScriptableObject
    {
        [field: SerializeField] public Material Ghost_Material { get; private set; }
        [field: SerializeField] public Material RedGhost_Material { get; private set; }
        [field: SerializeField] public Material Garden_Material { get; private set; }
        [field: SerializeField] public Material ClearMaterial { get; private set; }
        [field: SerializeField] public Material BusyMaterial { get; private set; }
    }
}