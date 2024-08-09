using System.Collections.Generic;
using System.Linq;
using Code.Digging.Grid;
using UnityEngine;

namespace Code.Building
{
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public GameObject Visual { get; private set; }
        public List<Tile> Tiles { get; private set; } = new();
        public Vector2Int Size { get; private set; }

        public void SetTiles(List<Tile> tiles)
        {
            Tiles = tiles;
        }
        
        public void SetSize(Vector2Int size)
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(scale.x * size.x, scale.z, scale.y * size.y);
            Size = size;
        }
        
        private void OnDestroy()
        {
            Tiles.ForEach(tile => tile.SetBuilding(null));
        }
    }
}