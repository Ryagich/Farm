using System;
using UnityEngine;

namespace Code.Digging.Grid
{
    public class Tiles
    {
        private Tile[,] tiles;
        private Vector2Int offset = Vector2Int.zero;

        public int MaxX => tiles.GetLength(0)-offset.x;
        public int MaxY => tiles.GetLength(1)-offset.y;
        public int MinX => -offset.x;
        public int MinY => -offset.y;

        public Tiles(int width, int height)
        {
            tiles = new Tile[width, height];
        }

        public Tile GetTile(int x, int y)
        {
            var realX = x + offset.x;
            var realY = y + offset.y;

            if (realX < 0 || realX >= tiles.GetLength(0)
             || realY < 0 || realY >= tiles.GetLength(1))
            {
                throw new IndexOutOfRangeException("Tile position out of bounds!");
            }

            return tiles[realX, realY];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            var realX = x + offset.x;
            var realY = y + offset.y;

            if (realX < 0 || realX >= tiles.GetLength(0)
             || realY < 0 || realY >= tiles.GetLength(1))
                throw new IndexOutOfRangeException("Tile position out of bounds!");

            tiles[realX, realY] = tile;
        }

        public void Resize(Vector2Int direction)
        {
            var newOffset = offset;

            if (direction.x < 0)
                newOffset = offset.WithXInt(offset.x + 1);
            if (direction.y < 0)
                newOffset = offset.WithYInt(offset.y + 1);

            var newTiles = new Tile[tiles.GetLength(0) + Mathf.Abs(direction.x),
                tiles.GetLength(1) + Mathf.Abs(direction.y)];

            for (var x = 0; x < tiles.GetLength(0); x++)
            for (var y = 0; y < tiles.GetLength(1); y++)
            {
                newTiles[x - offset.x + newOffset.x, y - offset.y + newOffset.y] = tiles[x, y];
            }
            offset = newOffset;
            tiles = newTiles;
        }
    }
}