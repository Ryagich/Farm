using System.Collections.Generic;
using UnityEngine;

namespace Code.Grid
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "Settings")]
    public class GridSettings : ScriptableObject
    {
        [field: SerializeField] public List<GridInfo> Info;
        [field: SerializeField] public float yOffset = .01f;
    }
}