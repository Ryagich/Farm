using System.Collections.Generic;
using System.Linq;
using Code.Game;
using UniRx;
using UnityEngine;

namespace Code.Digging.Grid.Extension
{
    //TODO: Разбить на методы.
    public class GridExtensionSpawner
    {
        private GameStateController gameStateController;
        private GridMediator gridMediator;
        private GridExtensionSettings extensionSettings;
        private GridSpawner spawner;
        
        private List<ExtensionPointer> extensions = new();

        private GridExtensionSpawner(GameStateController gameStateController,
                                     GridMediator gridMediator,
                                     GridExtensionSettings extensionSettings,
                                     GridSpawner spawner)
        {
            this.gameStateController = gameStateController;
            this.gridMediator = gridMediator;
            this.extensionSettings = extensionSettings;
            this.spawner = spawner;
            
            gameStateController.GameState.Subscribe(ShowExtension);
        }

        private void ShowExtension(GameStates state)
        {
            if (state != GameStates.Expansion)
            {
                HideExtension();
                return;
            }
            //var extensions = new List<ExtensionPointer>();
            var parents = spawner.Parents;
            for (var p = 0; p < parents.Count; p++)
            {
               var bottomTiles = GetHorizontalTilesForExtension(parents[p].Position.x, parents[p].Size.x, parents[p].Position.y, -1);
               var topTiles = GetHorizontalTilesForExtension(parents[p].Position.x, parents[p].Size.x, parents[p].Position.y + parents[p].Size.y - 1, 1);
               var leftTiles = GetVerticalTilesForExtension(parents[p].Position.y, parents[p].Size.y, parents[p].Position.x, -1);
               var rightTiles = GetVerticalTilesForExtension(parents[p].Position.y, parents[p].Size.y, parents[p].Position.x + parents[p].Size.x - 1, 1);

               if (bottomTiles.Count > 0)
               {
                   var groupedTiles = new List<List<Tile>>();
                   var currentGroup = new List<Tile> { bottomTiles[0] };
                   for (int i = 0; i < bottomTiles.Count; i++)
                   {
                       if (i + 1 < bottomTiles.Count
                        && bottomTiles[i].Index.y == bottomTiles[i + 1].Index.y
                        && bottomTiles[i].Index.x + 1 == bottomTiles[i + 1].Index.x)
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
                       var extension = MonoBehaviour.Instantiate(extensionSettings.ExpansionPref);
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
                   for (int i = 0; i < topTiles.Count; i++)
                   {
                       if (i + 1 < topTiles.Count
                        && topTiles[i].Index.y == topTiles[i + 1].Index.y
                        && topTiles[i].Index.x + 1 == topTiles[i + 1].Index.x)
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
                       var extension = MonoBehaviour.Instantiate(extensionSettings.ExpansionPref);
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
                   for (int i = 0; i < leftTiles.Count; i++)
                   {
                       if (i + 1 < leftTiles.Count
                        && leftTiles[i].Index.x == leftTiles[i + 1].Index.x
                        && leftTiles[i].Index.y + 1 == leftTiles[i + 1].Index.y)
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
                       var extension = MonoBehaviour.Instantiate(extensionSettings.ExpansionPref);
                       var first = group.First().Index;
                       var last = group.Last().Index;

                       extension.transform.position = new Vector3(parents[p].Position.x -.5f,
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
                   for (int i = 0; i < rightTiles.Count; i++)
                   {
                       if (i + 1 < rightTiles.Count
                        && rightTiles[i].Index.x == rightTiles[i + 1].Index.x
                        && rightTiles[i].Index.y + 1 == rightTiles[i + 1].Index.y)
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
                       var extension = MonoBehaviour.Instantiate(extensionSettings.ExpansionPref);
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
               MergeExtensions();
            }
        }
        
        private void MergeExtensions()
        {
            
        }
        
        private List<Tile> GetHorizontalTilesForExtension(int startPos, int size, int y, int offset)
        {
            var tiles = spawner.Tiles;
            var result = new List<Tile>();
            for (var x = startPos; x < size + startPos; x++)
            {
                if (tiles[x, y] != null
                  && (y + offset < 0
                  || y + offset >= tiles.GetLength(1)
                  || tiles[x, y + offset] == null))
                {
                    result.Add(tiles[x, y]);
                }
            }
            return result;
        }
        
        private List<Tile> GetVerticalTilesForExtension(int startPos, int size, int x, int offset)
        {
            var tiles = spawner.Tiles;
            var result = new List<Tile>();
            for (var y = startPos; y < size + startPos; y++)
            {
                if (tiles[x, y] != null
                 && (x + offset < 0
                 || x + offset >= tiles.GetLength(0)
                 || tiles[x + offset, y] == null))
                {
                    result.Add(tiles[x, y]);
                }
            }
            return result;
        }
        
        private void HideExtension()
        {
            foreach (var extension in extensions)
            {
                MonoBehaviour.Destroy(extension.gameObject);
            }
            extensions = new List<ExtensionPointer>();
        }
    }
}