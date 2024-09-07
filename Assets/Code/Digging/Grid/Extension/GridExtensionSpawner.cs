using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Digging.Grid.Extension
{
    //TODO: Разбить на методы.
    public class GridExtensionSpawner
    {
        private readonly GameStateController gameStateController;
        private readonly GridController gridController;
        private readonly GridExtensionSettings extensionSettings;

        private List<ExtensionPointer> extensions = new();
        
        private GridExtensionSpawner(GameStateController gameStateController,
                                     GridController gridController,
                                     GridExtensionSettings extensionSettings)
        {
            this.gameStateController = gameStateController;
            this.gridController = gridController;
            this.extensionSettings = extensionSettings;
            
            gameStateController.GameState.Subscribe(ShowExtension);
        }

        public void OnExtension()
        {
            HideExtension();
            ShowExtension(gameStateController.GameState.Value);
        }
        
        private void ShowExtension(GameStates state)
        {
            if (state != GameStates.Expansion)
            {
                HideExtension();
                return;
            }
            var parents = gridController.Parents;
            for (var p = 0; p < parents.Count; p++)
            {
                var bottomTiles =
                    GetHorizontalTilesForExtension(parents[p].Position.x, parents[p].Size.x,
                                                                parents[p].Position.y, -1);
                var topTiles =
                    GetHorizontalTilesForExtension(parents[p].Position.x, parents[p].Size.x,
                                                              parents[p].Position.y + parents[p].Size.y - 1, 1);
                var leftTiles =
                    GetVerticalTilesForExtension(parents[p].Position.y, parents[p].Size.y, 
                                                                parents[p].Position.x, -1);
                var rightTiles = 
                    GetVerticalTilesForExtension(parents[p].Position.y, parents[p].Size.y,
                                                              parents[p].Position.x + parents[p].Size.x - 1, 1);
                if (bottomTiles.Count > 0)
                {
                    var groupedTiles = new List<List<Tile>>();
                    var currentGroup = new List<Tile> { bottomTiles[0] };
                    for (var i = 0; i < bottomTiles.Count; i++)
                    {
                        if (i + 1 < bottomTiles.Count
                         && bottomTiles[i].Index.y == bottomTiles[i + 1].Index.y
                         && bottomTiles[i].Index.x + 1 == bottomTiles[i + 1].Index.x 
                         && bottomTiles[i].Type == bottomTiles[i + 1].Type)
                        {
                            currentGroup.Add(bottomTiles[i + 1]);
                        }
                        else
                        {
                            groupedTiles.Add(currentGroup);
                            if (i + 1 < bottomTiles.Count)
                                currentGroup = new List<Tile> { bottomTiles[i + 1] };
                        }
                    }
                    foreach (var group in groupedTiles)
                    {
                        var extension = Object.Instantiate(extensionSettings.ExpansionPref);
                        var first = group.First().Index;
                        var last = group.Last().Index;

                        extension.transform.position = new Vector3(group.Count > 1
                                                                       ? (float)(first.x + last.x + 1) / 2
                                                                       : first.x + .5f,
                                                                   -.25f,
                                                                   parents[p].Position.y - .5f);
                        extension.SetValues(Vector2Int.down, group);
                        extensions.Add(extension);
                    }
                }
                if (topTiles.Count > 0)
                {
                    var groupedTiles = new List<List<Tile>>();
                    var currentGroup = new List<Tile> { topTiles[0] };
                    for (var i = 0; i < topTiles.Count; i++)
                    {
                        if (i + 1 < topTiles.Count
                         && topTiles[i].Index.y == topTiles[i + 1].Index.y
                         && topTiles[i].Index.x + 1 == topTiles[i + 1].Index.x
                         && topTiles[i].Type == topTiles[i + 1].Type)
                        {
                            currentGroup.Add(topTiles[i + 1]);
                        }
                        else
                        {
                            groupedTiles.Add(currentGroup);
                            if (i + 1 < topTiles.Count)
                                currentGroup = new List<Tile> { topTiles[i + 1] };
                        }
                    }
                    foreach (var group in groupedTiles)
                    {
                        var extension = Object.Instantiate(extensionSettings.ExpansionPref);
                        var first = group.First().Index;
                        var last = group.Last().Index;

                        extension.transform.position = new Vector3(group.Count > 1
                                                                       ? (float)(first.x + last.x + 1) / 2
                                                                       : first.x + .5f,
                                                                   -.25f,
                                                                   parents[p].Position.y + parents[p].Size.y + .5f);
                        extension.SetValues(Vector2Int.up, group);
                        extensions.Add(extension);
                    }
                }
                if (leftTiles.Count > 0)
                {
                    var groupedTiles = new List<List<Tile>>();
                    var currentGroup = new List<Tile> { leftTiles[0] };
                    for (var i = 0; i < leftTiles.Count; i++)
                    {
                        if (i + 1 < leftTiles.Count
                         && leftTiles[i].Index.x == leftTiles[i + 1].Index.x
                         && leftTiles[i].Index.y + 1 == leftTiles[i + 1].Index.y
                         && leftTiles[i].Type == leftTiles[i + 1].Type)
                        {
                            currentGroup.Add(leftTiles[i + 1]);
                        }
                        else
                        {
                            groupedTiles.Add(currentGroup);
                            if (i + 1 < leftTiles.Count)
                                currentGroup = new List<Tile> { leftTiles[i + 1] };
                        }
                    }
                    foreach (var group in groupedTiles)
                    {
                        var extension = Object.Instantiate(extensionSettings.ExpansionPref);
                        var first = group.First().Index;
                        var last = group.Last().Index;

                        extension.transform.position = new Vector3(parents[p].Position.x - .5f,
                                                                   -.25f,
                                                                   group.Count > 1
                                                                       ? (float)(first.y + last.y + 1) / 2
                                                                       : first.y + .5f);
                        extension.SetValues(Vector2Int.left, group);
                        extensions.Add(extension);
                    }
                }
                if (rightTiles.Count > 0)
                {
                    var groupedTiles = new List<List<Tile>>();
                    var currentGroup = new List<Tile> { rightTiles[0] };
                    for (var i = 0; i < rightTiles.Count; i++)
                    {
                        if (i + 1 < rightTiles.Count
                         && rightTiles[i].Index.x == rightTiles[i + 1].Index.x
                         && rightTiles[i].Index.y + 1 == rightTiles[i + 1].Index.y
                         && rightTiles[i].Type == rightTiles[i + 1].Type)
                        {
                            currentGroup.Add(rightTiles[i + 1]);
                        }
                        else
                        {
                            groupedTiles.Add(currentGroup);
                            if (i + 1 < rightTiles.Count)
                                currentGroup = new List<Tile> { rightTiles[i + 1] };
                        }
                    }
                    foreach (var group in groupedTiles)
                    {
                        var extension = Object.Instantiate(extensionSettings.ExpansionPref);
                        var first = group.First().Index;
                        var last = group.Last().Index;

                        extension.transform.position = new Vector3(parents[p].Position.x + parents[p].Size.x + .5f,
                                                                   -.25f,
                                                                   group.Count > 1
                                                                       ? (float)(first.y + last.y + 1) / 2
                                                                       : first.y + .5f);
                        extension.SetValues(Vector2Int.right, group);
                        extensions.Add(extension);
                    }
                }
            }
            MergeExtensions();
        }

        private void MergeExtensions()
        {
            var extensionsByDirection = extensions.GroupBy(ext => ext.Direction);
            foreach (var byDir in extensionsByDirection)
            {
                var groups =
                    GetGroups(byDir, (a, b) => CanMerge(byDir.Key, a.Tiles, b.Tiles) 
                                            && a.Tiles.First().Type == b.Tiles.First().Type);
                foreach (var group in groups)
                {
                    var tiles = group.SelectMany(pointer => pointer.Tiles).Distinct().ToList();

                    foreach (var pointer in group)
                    {
                        extensions.Remove(pointer);
                        Object.Destroy(pointer.gameObject);
                    }
                    
                    var extension = Object.Instantiate(extensionSettings.ExpansionPref);
                    var pos = tiles.Aggregate(Vector2.zero, (acc, tile) => acc + tile.Index) / tiles.Count;
                    pos += byDir.Key + Vector2.one * 0.5f;

                    extension.transform.position = new Vector3(pos.x,
                                                               -.25f,
                                                               pos.y);
                    extension.SetValues(byDir.Key, tiles);
                    extensions.Add(extension);
                }
            }

            return;

            bool CanMerge(Vector2Int direction, IReadOnlyCollection<Tile> a, IReadOnlyCollection<Tile> b)
            {
                var coordA = GetRotatedCoordinates(direction, a.First().Index);
                var coordB = GetRotatedCoordinates(direction, b.First().Index);
                if (coordA.y != coordB.y)
                    return false;
                var aMax = a.Max(t => GetRotatedCoordinates(direction, t.Index).x);
                var aMin = a.Min(t => GetRotatedCoordinates(direction, t.Index).x);
                var bMax = b.Max(t => GetRotatedCoordinates(direction, t.Index).x);
                var bMin = b.Min(t => GetRotatedCoordinates(direction, t.Index).x);
                return aMax + 1 >= bMin || bMax + 1 >= aMin;
            }

            Vector2Int GetRotatedCoordinates(Vector2Int direction, Vector2Int position)
            {
                if (direction == Vector2Int.up ||
                    direction == Vector2Int.down)
                {
                    return new Vector2Int(position.x, position.y);
                }
                if (direction == Vector2Int.left ||
                    direction == Vector2Int.right)
                {
                    return new Vector2Int(position.y, position.x);
                }
                throw new Exception("Unknown direction");
            }

            IEnumerable<IEnumerable<T>> GetGroups<T>(IEnumerable<T> elements, Func<T, T, bool> compare)
            {
                var groups = new List<List<T>>();
                foreach (var e in elements)
                {
                    if (groups.Count < 1)
                    {
                        groups.Add(new List<T> { e });
                        continue;
                    }

                    var isInGroup = false;

                    foreach (var group in groups.Where(group => compare(group[0], e)))
                    {
                        group.Add(e);
                        isInGroup = true;
                        break;
                    }

                    if (!isInGroup)
                        groups.Add(new List<T> { e });
                }
                return groups;
            }
        }

        private List<Tile> GetHorizontalTilesForExtension(int startPos, int size, int y, int offset)
        {
            var tiles = gridController.Tiles;
            var result = new List<Tile>();
            for (var x = startPos; x < size + startPos; x++)
            {
                if (tiles.GetTile(x, y) != null
                 && (y + offset < tiles.MinY
                  || y + offset >= tiles.MaxY
                  || tiles.GetTile(x, y + offset) == null))
                {
                    result.Add(tiles.GetTile(x, y));
                }
            }
            return result;
        }

        private List<Tile> GetVerticalTilesForExtension(int startPos, int size, int x, int offset)
        {
            var tiles = gridController.Tiles;
            var result = new List<Tile>();
            for (var y = startPos; y < size + startPos; y++)
            {
                if (tiles.GetTile(x, y) != null
                 && (x + offset < tiles.MinX
                  || x + offset >= tiles.MaxX
                  || tiles.GetTile(x + offset, y) == null))
                {
                    result.Add(tiles.GetTile(x, y));
                }
            }
            return result;
        }

        private void HideExtension()
        {
            foreach (var extension in extensions)
            {
                Object.Destroy(extension.gameObject);
            }
            extensions = new List<ExtensionPointer>();
        }
    }
}