using UnityEngine;

namespace Code.Digging.Grid
{
    public class GridParent : MonoBehaviour
    {
        public Vector2Int Position { get; private set; }
        public Vector2Int Size { get; private set; }
        public TileTypes Type { get; private set; }

        public void Initialize(Vector2Int position,
                               Vector2Int size,
                               TileTypes type)
        {
            Position = position;
            Size = size;
            Type = type;
        }
    }
}
