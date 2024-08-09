using System;
using Code.Digging.Grid;
using UnityEngine;

namespace Code.Grid
{
    [Serializable]
    public class GridInfo
    {
        [field: SerializeField] public Vector3Int Position { get; private set; }
        [field: SerializeField]  public Vector2Int Size { get; private set; }
        [field: SerializeField]   public TileTypes Type { get; private set; }
    }
}