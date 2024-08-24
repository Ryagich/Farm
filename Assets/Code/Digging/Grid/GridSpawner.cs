using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Digging.Grid
{
    public class GridSpawner
    {
        public Tile[,] Tiles { get; private set; }
        public List<GridParent> Parents { get; private set; } = new();
        public List<GameObject> Planes { get; private set; } = new();
        private readonly GridSettings settings;

        private GridSpawner(GridSettings settings)
        {
            this.settings = settings;
        }
        
        public List<GridParent> MergeParents(List<GridParent> parents)
        {
            var i = 0;
            while (i < parents.Count)
            {
                var startParentsCount = parents.Count;
                for (var j = 0; j < parents.Count; j++)
                {
                    if (startParentsCount != parents.Count
                     || parents[i] == parents[j])
                        continue;
                    if (TryMerge(parents[i], parents[j], out var newParent))
                    {
                        var a = parents[i];
                        var b =parents[j];
                        parents[i] = newParent;
                        parents.Remove(b);
                        Object.Destroy(a.gameObject);
                        Object.Destroy(b.gameObject);
                        break;
                    }
                }
                if (startParentsCount == parents.Count)
                {
                    i++;
                }
            }
            return parents;
        }

        private bool TryMerge(GridParent a, GridParent b, out GridParent result)
        {
            result = null;
            if (a.Position.y == b.Position.y
             && a.Size.y == b.Size.y
             && (a.Position.x + a.Size.x == b.Position.x
              || b.Position.x + b.Size.x == a.Position.x))
            {
                var x = a.Position.x < b.Position.x ? a.Position.x : b.Position.x;
                result = Object.Instantiate(settings.Parent);
                result.Initialize(new Vector2Int(x, a.Position.y),
                                  new Vector2Int(a.Size.x + b.Size.x, a.Size.y),
                                  a.Type);
                result.transform.position = new Vector3(x, 0, a.Position.y);
                return true;
            }
            if (a.Position.x == b.Position.x
             && a.Size.x == b.Size.x
             && (a.Position.y + a.Size.y == b.Position.y
              || b.Position.y + b.Size.y == a.Position.y))
            {
                var y = a.Position.y < b.Position.y ? a.Position.y : b.Position.y;
                result = Object.Instantiate(settings.Parent);
                result.Initialize(new Vector2Int(a.Position.x, y),
                                  new Vector2Int(a.Size.x, a.Size.y + b.Size.y),
                                  a.Type);
                result.transform.position = new Vector3(a.Position.x, 0, y);
                return true;
            }
            return false;
        }
        
        public void CreateGrid()
        {
            Tiles = GetEmptyTiles();
            var startPosition = settings.Info[0].Position;

            foreach (var info in settings.Info)
            {
                var parent = Object.Instantiate(settings.Parent);
               
                parent.transform.position = new Vector3(info.Position.x, 0, info.Position.y);
                parent.Initialize(new Vector2Int(info.Position.x, info.Position.y),
                                  new Vector2Int(info.Size.x, info.Size.y),
                                  info.Type);
                for (var x = 0; x < info.Size.x; x++)
                {
                    for (var y = 0; y < info.Size.y; y++)
                    {
                        var index = new Vector2Int(x + startPosition.x + info.Position.x,
                                                   y + startPosition.y + info.Position.y);
                        var tile = new Tile(index, info.Type);
                        if (Tiles[tile.Index.x, tile.Index.y] != null)
                        {
                            throw new
                                ArgumentOutOfRangeException("На этом месте уже есть тайл. Перепроверь позиции гридов");
                        }
                        Tiles[tile.Index.x, tile.Index.y] = tile;
                    }
                }
                Parents.Add(parent);
            }
            Parents = MergeParents(Parents);
            InstantiatePlanes();
        }

        public void Extent(ExtensionPointer pointer)
        {
            var direction = pointer.Direction;
            var tiles = new Tile[Tiles.GetLength(0) + Math.Abs(direction.x), Tiles.GetLength(1) + Math.Abs(direction.y)];
            {
                
            }
        }
        
        private Tile[,] GetEmptyTiles()
        {
            var maxX = settings.Info.Max(info => info.Position.x + info.Size.x);
            var maxY = settings.Info.Max(info => info.Position.y + info.Size.y);
            return new Tile[maxX, maxY];
        }
        
        private void InstantiatePlanes()
        {
            foreach (var plane in Planes)
            {
                Object.Destroy(plane.gameObject);
            }
            Planes.Clear();
            foreach (var parent in Parents)
            {
                var plane = Object.Instantiate(settings.TilePref);
                var gridPlaneTransform = plane.transform;
                gridPlaneTransform.position = new Vector3(parent.Position.x, 0, parent.Position.y);
                gridPlaneTransform.localScale = new Vector3(-parent.Size.x, gridPlaneTransform.localScale.y, parent.Size.y);
                Planes.Add(plane);
            }
        }
    }
}