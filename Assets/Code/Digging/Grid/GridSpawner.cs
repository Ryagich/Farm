using System;
using System.Collections.Generic;
using System.Linq;
using Code.Grid;
using UnityEngine;

namespace Code.Digging.Grid
{
    public class GridSpawner
    {
        private readonly GridSettings settings;

        private GridSpawner(GridSettings settings)
        {
            this.settings = settings;
        }

        private Tile[,] GetEmptyTiles()
        {
            var maxX = settings.Info.Max(info => info.Position.x + info.Size.x);
            var maxY = settings.Info.Max(info => info.Position.y + info.Size.y);
            return new Tile[maxX, maxY];
        }

        public void CreateGrid(out Tile[,] tiles, out List<GridParent> parents, out List<GameObject> planes)
        {
            tiles = GetEmptyTiles();
            parents = new();
            planes = new();
            var startPosition = settings.Info[0].Position;

            foreach (var info in settings.Info)
            {
                var parent = MonoBehaviour.Instantiate(settings.Parent);
                var plane = MonoBehaviour.Instantiate(settings.TilePref);
                var gridPlaneTransform = plane.transform;
                gridPlaneTransform.position = new Vector3(info.Position.x,0,info.Position.y);
                gridPlaneTransform.localScale = new Vector3(-info.Size.x, gridPlaneTransform.localScale.y, info.Size.y);
                
                parent.transform.position = new Vector3(info.Position.x,0,info.Position.y);
                parent.SetSize(new Vector2Int(info.Size.x, info.Size.y));
                parent.SetType(info.Type);
                
                for (var x = 0; x < info.Size.x; x++)
                {
                    for (var y = 0; y < info.Size.y; y++)
                    {
                        var index = new Vector2Int(x + startPosition.x + info.Position.x, y + startPosition.y + +info.Position.y);
                        var tile = new Tile(index, info.Type);
                        if (tiles[tile.Index.x, tile.Index.y] != null)
                        {
                            throw new ArgumentOutOfRangeException("На этом месте уже есть тайл. Перепроверь позиции гридов");
                        } 
                        tiles[tile.Index.x, tile.Index.y] = tile;
                    }
                }
                parents.Add(parent);
                planes.Add(plane);
            }
        }
    }
}