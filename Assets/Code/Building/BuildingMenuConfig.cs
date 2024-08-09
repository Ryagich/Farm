using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Building
{
    [CreateAssetMenu(fileName = "BuildingMenuConfig", menuName = "BuildingMenuConfig")]
    public class BuildingMenuConfig : ScriptableObject
    {
        [field: SerializeField] public VisualTreeAsset MenuTree { get; private set; }
        [field: SerializeField] public VisualTreeAsset ButtonTree { get; private set; }
        [field: SerializeField] public string MoveButtonName { get; private set; }
        [field: SerializeField] public string DeleteButtonName { get; private set; }
    }
}