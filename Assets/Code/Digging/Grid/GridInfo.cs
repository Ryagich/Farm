using System;
using Code.Digging.Grid;
using UnityEngine;

namespace Code.Grid
{
    [Serializable]
    public class GridInfo
    {
        [field: SerializeField] public Vector2Int Position { get; private set; }
        [field: SerializeField]  public Vector2Int Size { get; private set; }
        [field: SerializeField]   public TileTypes Type { get; private set; }

        public GridInfo(Vector2Int position,Vector2Int size, TileTypes type)
        {
            Position = position;
            Size = size;
            Type = type;
        }
    }
}