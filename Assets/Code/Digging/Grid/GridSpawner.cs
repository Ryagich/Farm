using System;
using System.Collections.Generic;
using Code.Digging.Grid;
using UnityEngine;

namespace Code.Grid
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
            var startPosition = settings.Info[0].Position;
            var endPosition = settings.Info[^1].Position;
            var endSize = settings.Info[^1].Size;
            return new Tile[endPosition.x - startPosition.x + endSize.x,
                            endPosition.z - startPosition.z + endSize.y];
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
                var localPosition = startPosition;
                var plane = MonoBehaviour.Instantiate(settings.TilePref);
                var gridPlaneTransform = plane.transform;
                gridPlaneTransform.position = info.Position;
                gridPlaneTransform.localScale = new Vector3(-info.Size.x, gridPlaneTransform.localScale.y, info.Size.y);
                
                parent.transform.position = info.Position;
                parent.SetSize(new Vector2Int(info.Size.x, info.Size.y));
                parent.SetType(info.Type);
                
                for (var x = 0; x < info.Size.x; x++)
                {
                    for (var y = 0; y < info.Size.y; y++)
                    {
                        var index = new Vector2Int(x + localPosition.x + info.Position.x, y + localPosition.z + +info.Position.z);
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