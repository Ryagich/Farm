using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Input
{
    [CreateAssetMenu(fileName = "Input", menuName = "Input")]
    public class InputKeys : ScriptableObject
    {
        [field: SerializeField] public InputActionReference LeftMouse { get; private set; }
        [field: SerializeField] public InputActionReference RightMouse { get; private set; }
        [field: SerializeField] public InputActionReference Control { get; private set; }
        [field: SerializeField] public InputActionReference R { get; private set; }
    }
}
