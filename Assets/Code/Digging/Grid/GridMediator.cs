using System;
using System.Collections.Generic;
using Code.Digging.Grid;
using Code.Digging.Grid.Utils;
using Code.Game;
using Code.Grid.Utils;
using UniRx;
using UnityEngine;

namespace Code.Grid
{
    public class GridMediator
    {
        private GridSettings settings;
        private GridSpawner spawner;
        private VisualGridSeparation separation;
        private GridVisualizationForGarden visualizationForGarden;
        
        private Tile[,] tiles;
        private List<GridParent> parents;
        private List<GameObject> planes;
        private List<GameObject> gardenTiles;

        private GridMediator(GridSettings settings,
                             GridSpawner spawner,
                             VisualGridSeparation separation,
                             GridVisualizationForGarden visualizationForGarden,
                             GameStateController gameStateController)
        {
           this.settings = settings;
           this.spawner = spawner;
           this.separation = separation;
           this.visualizationForGarden = visualizationForGarden;

           spawner.CreateGrid(out Tile[,] tiles, out List<GridParent> parents, out List<GameObject> planes);
           this.tiles = tiles;
           this.parents = parents;
           this.planes = planes;
           
           gameStateController.GameState.Subscribe(OnChangedGameState);
        }

        public void Cancel()
        {
            foreach (var tile in gardenTiles)
            {
               MonoBehaviour.Destroy(tile);
            }
            gardenTiles.Clear();
        }

        public void SetGardenTiles(Vector2Int size)
        {
            gardenTiles = new List<GameObject>();
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    var tile = MonoBehaviour.Instantiate(settings.TilePref);
                    tile.SetActive(false);
                    gardenTiles.Add(tile);
                }
            }
        }

        public bool CanPlace(Vector2Int size)
        {
           var currTiles = tiles.GetTilesAround(GridRaycaster.GetRaycastPosition(),size);
           if (currTiles.Count < size.x * size.y)
           {
               return false;
           }
           foreach (var tile in currTiles)
           {
               if (tile.State == TileState.Busy)
               {
                   return false;
               }
           }
           return true;
        }
        
        public void ChangeGardenTilesState(Vector2Int size)
        {
            var position = GridRaycaster.GetRaycastPosition();

            foreach (var tile in tiles.GetTilesAround(position, size))
            {
                tile.ChangeTileState(TileState.Busy);
            }
        }
        
        public void VisualizationTiles(Vector2Int size, Vector2Int position)
        {
            visualizationForGarden.SetTilesPosition(tiles.GetTilesAround(position, size),gardenTiles);
            visualizationForGarden.PaintTiles(tiles.GetTilesAround(position, size),gardenTiles, settings);
        }
        
        private void OnChangedGameState(GameStates currentState)
        {
            switch (currentState)
            {
                case GameStates.Game:
                    separation.UnDraw(parents);
                    break;
                case GameStates.Redactor:
                    separation.Draw(parents);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentState));
            }
        }
    }
}