using System.Collections.Generic;
using Code.Grid;
using UnityEngine;

namespace Code.Digging.Grid.Utils
{
    public static class TileArrayExtension
    {
        public static List<Tile> GetTilesAround(this Tiles tiles, Vector2Int position, Vector2Int size)
        {
            var result = new List<Tile>();
            for (var x = position.x - size.x + 1; x < position.x + 1; x++)
            {
                if (x < tiles.MinX || x >= tiles.MaxX)
                {
                    continue;
                }
                for (var y = position.y - size.y + 1; y < position.y + 1; y++)
                {
                    if (y < tiles.MinY || y >= tiles.MaxY)
                    {
                        continue;
                    }
                    if (tiles.GetTile(x, y) != null)
                    {
                        result.Add(tiles.GetTile(x, y));
                    }
                }
            }
            return result;
        }
    }
}