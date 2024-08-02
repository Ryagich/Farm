using System.Collections.Generic;
using Code.Grid;
using UnityEngine;

namespace Code.Digging.Grid
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "GridSettings")]
    public class GridSettings : ScriptableObject
    {
        [field: SerializeField] public List<GridInfo> Info { get; private set; }
        [field: SerializeField] public float yOffset { get; private set; } = .01f;
        [field: SerializeField] public GameObject TilePref { get; private set; }
        [field: SerializeField] public GridParent Parent { get; private set; }
        [field: SerializeField] public Material ClearMaterial { get; private set; }
        [field: SerializeField] public Material BusyMaterial { get; private set; }
        private void OnValidate()
        {
            Info.Sort( (a, b) => a.Position.x != b.Position.x
                    ? a.Position.x.CompareTo(b.Position.x)
                    : a.Position.y.CompareTo(b.Position.y));
        }
    }
}