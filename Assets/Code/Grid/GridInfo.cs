using System;
using UnityEngine;

namespace Code.Grid
{
    [Serializable]
    public class GridInfo
    {
        [field: SerializeField] public Vector3Int StartPoint { get; private set; }
        [field: SerializeField] public Vector2Int Size { get; private set; }
    }
}