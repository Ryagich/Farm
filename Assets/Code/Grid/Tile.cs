using System;
using UnityEngine;

namespace Code.Grid
{
    [Serializable]
    public class Tile
    {
        [field: SerializeField] public TileTypes Type { get; private set; }
        [field: SerializeField] public TileState CurrentState { get; private set; }
        [field: SerializeField] public Vector2Int Index { get; private set; }

        public Tile(Vector2Int index,
                    TileTypes type, TileState state)
        {
            Index = index;
        }
    }
    
    public enum TileTypes
    {
        Ground,
        Shop,
    }
    
    public enum TileState
    {
        Clear,
        Busy,
    }
}