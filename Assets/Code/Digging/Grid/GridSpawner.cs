using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Digging.Grid
{
    public class GridSpawner
    {
        public event Action Extented;
        public Tiles Tiles { get; private set; }
        public List<GridParent> Parents { get; private set; } = new();
        public List<GameObject> Planes { get; private set; } = new();
        private readonly GridSettings settings;

        private GridSpawner(GridSettings settings)
        {
            this.settings = settings;
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
                        if (Tiles.GetTile(tile.Index.x, tile.Index.y) != null)
                        {
                            throw new
                                ArgumentOutOfRangeException("На этом месте уже есть тайл. Перепроверь позиции гридов");
                        }
                        Tiles.SetTile(tile.Index.x, tile.Index.y, tile);
                    }
                }
                Parents.Add(parent);
            }
            Parents = MergeParents(Parents);
            UpdatePlanes();
        }

        public void Extent(ExtensionPointer pointer)
        {
            var direction = pointer.Direction;
            Tiles.Resize(direction);
            for (var i = 0; i < pointer.Tiles.Count; i++)
            {
                var index = pointer.Tiles[i].Index + direction;
                var tile = new Tile(index, pointer.Tiles[i].Type);
                Tiles.SetTile(index.x, index.y, tile);
                var parent = Object.Instantiate(settings.Parent);
                parent.transform.position = new Vector3(index.x, 0, index.y);
                parent.Initialize(new Vector2Int(index.x, index.y),
                                  new Vector2Int(1, 1),
                                  pointer.Tiles.First().Type);
                Parents.Add(parent);
            }
            
            MergeParents(Parents);
            UpdatePlanes();
            Extented?.Invoke();
        }
        
        private List<GridParent> MergeParents(List<GridParent> parents)
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
                    if (parents[i].Type == parents[j].Type 
                     && TryMerge(parents[i], parents[j], out var newParent))
                    {
                        var a = parents[i];
                        var b = parents[j];
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
            var canMergeHorizontally = a.Position.y == b.Position.y && a.Size.y == b.Size.y
                                        && (a.Position.x + a.Size.x == b.Position.x || b.Position.x + b.Size.x == a.Position.x);
            var canMergeVertically = a.Position.x == b.Position.x && a.Size.x == b.Size.x
                                      && (a.Position.y + a.Size.y == b.Position.y || b.Position.y + b.Size.y == a.Position.y);

            if (canMergeHorizontally || canMergeVertically)
            {
                var x = Math.Min(a.Position.x, b.Position.x);
                var y = Math.Min(a.Position.y, b.Position.y);

                result = Object.Instantiate(settings.Parent);
                result.Initialize(new Vector2Int(x, y),
                                  new Vector2Int(canMergeHorizontally ? a.Size.x + b.Size.x : a.Size.x,
                                                 canMergeVertically ? a.Size.y + b.Size.y : a.Size.y),
                                  a.Type);
                result.transform.position = new Vector3(x, 0, y);
                return true;
            }
    
            return false;
        }
        
        private Tiles GetEmptyTiles()
        {
            var maxX = settings.Info.Max(info => info.Position.x + info.Size.x);
            var maxY = settings.Info.Max(info => info.Position.y + info.Size.y);
            return new Tiles(maxX, maxY);
        }
        
        private void UpdatePlanes()
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