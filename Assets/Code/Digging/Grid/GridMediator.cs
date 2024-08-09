using System;
using System.Collections.Generic;
using Code.Digging.Grid.Utils;
using Code.Game;
using Code.Grid;
using UniRx;
using UnityEngine;

namespace Code.Digging.Grid
{
    public class GridMediator
    {
        private GridSettings settings;
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
           var currTiles = tiles.GetTilesAround(GridRaycaster.GetRaycastIntPosition(),size);
           if (currTiles.Count < size.x * size.y)
           {
               return false;
           }
           foreach (var tile in currTiles)
           {
               if (!tile.IsFree)
               {
                   return false;
               }
           }
           return true;
        }
        
        public List<Tile> ChangeGardenTilesState(Vector2Int size, Building.Building building)
        {
            var position = GridRaycaster.GetRaycastIntPosition();
            var tilesAround = tiles.GetTilesAround(position, size);
            
            foreach (var tile in tilesAround)
            {
                tile.SetBuilding(building);
            }
            return tilesAround;
        }
        
        public void VisualizationTiles(Vector2Int size, Vector2Int position)
        {
            visualizationForGarden.SetTilesPosition(tiles.GetTilesAround(position, size),gardenTiles);
            visualizationForGarden.PaintTiles(tiles.GetTilesAround(position, size),gardenTiles, settings);
        }

        public bool TryGetTileOnMouse(out Tile tile)
        {
            tile = null;
            var position = GridRaycaster.GetRaycastIntPosition();
            if (position.x >= 0 && position.x < tiles.GetLength(0)
             && position.y >= 0 && position.y < tiles.GetLength(1))
            {
                tile = tiles[position.x, position.y];
                return true;
            }
            return false;
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