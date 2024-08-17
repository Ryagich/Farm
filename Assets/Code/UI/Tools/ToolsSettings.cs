using UnityEngine;
using UnityEngine.UIElements;

namespace Code.UI.Tools
{
    [CreateAssetMenu(fileName = "ToolsSettings", menuName = "ToolsSettings")]
    public class ToolsSettings : ScriptableObject
    {
        [field: SerializeField] public float MinHeight { get; private set; }
        [field: SerializeField] public float MaxHeight { get; private set; }
        [field: SerializeField] public float Translation { get; private set; }= .2f;
        [field: SerializeField] public EasingMode Easing{ get; private set; }
    }
}