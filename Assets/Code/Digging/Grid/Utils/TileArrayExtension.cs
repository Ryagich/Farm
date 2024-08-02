using System.Collections.Generic;
using Code.Grid;
using UnityEngine;

namespace Code.Digging.Grid.Utils
{
    public static class TileArrayExtension
    {
        public static List<Tile> GetTilesAround(this Tile[,] tiles, Vector2Int position, Vector2Int size)
        {
            var result = new List<Tile>();
            for (var x = position.x - size.x + 1; x < position.x + 1; x++)
            {
                if (x < 0 || x >= tiles.GetLength(0))
                {
                    continue;
                }
                for (var y = position.y - size.y + 1; y < position.y + 1; y++)
                {
                    if (y < 0 || y >= tiles.GetLength(1))
                    {
                        continue;
                    }
                    if (tiles[x, y] != null)
                    {
                        result.Add(tiles[x, y]);
                    }
                }
            }
            return result;
        }
    }
}