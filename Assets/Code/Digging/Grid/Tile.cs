using System;
using UnityEngine;

namespace Code.Digging.Grid
{
    [Serializable]
    public class Tile
    {
        [field: SerializeField] public TileTypes Type { get; private set; }
        public bool IsFree => Building == null;
        [field: SerializeField] public Vector2Int Index { get; private set; }

        public Building.Building Building { get; private set; } = null;
        
        public Tile(Vector2Int index, TileTypes type,Building.Building building = null)
        {
            Index = index;
            Type = type;
            Building = building;
        }

        public void SetBuilding(Building.Building newBuilding)
        {
            Building = newBuilding;
        }
    }
    
    public enum TileTypes
    {
        Ground,
        Shop,
        Wall,
    }
}