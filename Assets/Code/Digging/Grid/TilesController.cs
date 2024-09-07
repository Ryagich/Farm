using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Digging.Grid
{
    public class TilesController
    {
        public Tiles Tiles { get; private set; }

        private readonly GridSettings settings;

        public TilesController(GridSettings settings)
        {
            this.settings = settings;
            Tiles = CreateGrid();
        }
        
        private Tiles CreateGrid()
        {
            var tiles = GetEmptyTiles();
            var startPosition = settings.Info[0].Position;
            foreach (var info in settings.Info)
                for (var x = 0; x < info.Size.x; x++)
                    for (var y = 0; y < info.Size.y; y++)
                    {
                        var index = new Vector2Int(x + startPosition.x + info.Position.x,
                                                   y + startPosition.y + info.Position.y);
                        var tile = new Tile(index, info.Type);
                        if (tiles.GetTile(tile.Index.x, tile.Index.y) != null)
                        {
                            throw new
                                ArgumentOutOfRangeException("На этом месте уже есть тайл. Перепроверь позиции гридов");
                        }
                        tiles.SetTile(tile.Index.x, tile.Index.y, tile);
                    }
            return tiles;
        }

        public void Extent(ExtensionPointer pointer, Tiles tiles, List<GridParent> parents)
        {
            var direction = pointer.Direction;
            tiles.Resize(direction);
            for (var i = 0; i < pointer.Tiles.Count; i++)
            {
                var index = pointer.Tiles[i].Index + direction;
                var tile = new Tile(index, pointer.Tiles[i].Type);
                tiles.SetTile(index.x, index.y, tile);
                var parent = Object.Instantiate(settings.Parent);
                parent.transform.position = new Vector3(index.x, 0, index.y);
                parent.Initialize(new Vector2Int(index.x, index.y), new Vector2Int(1, 1),
                                  pointer.Tiles.First().Type);
                parents.Add(parent);
            }
        }
        
        private Tiles GetEmptyTiles()
        {
            var maxX = settings.Info.Max(info => info.Position.x + info.Size.x);
            var maxY = settings.Info.Max(info => info.Position.y + info.Size.y);
            return new Tiles(maxX, maxY);
        }
    }
}