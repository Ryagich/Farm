using System.Collections.Generic;
using UnityEngine;

namespace Code.Grid
{
    public class GridParent : MonoBehaviour
    {
        public Vector2Int Size { get; private set; }
        public TileTypes Type { get; private set; }

        public void SetSize(Vector2Int size)
        {
            Size = size;
        }
        
        public void SetType(TileTypes type)
        {
            Type = type;
        }
    }
}
