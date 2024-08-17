using UnityEngine;

namespace Code.Digging.Grid
{
    [CreateAssetMenu(fileName = "GridExpansionSettings", menuName = "GridExpansionSettings")]
    public class GridExtensionSettings : ScriptableObject
    {
        [field: SerializeField] public ExtensionPointer ExpansionPref { get; private set; }
    }
}