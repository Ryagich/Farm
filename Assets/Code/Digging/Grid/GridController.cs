using System.Collections.Generic;
using Code.Digging.Grid.Utils;
using Code.Game;
using Code.Grid;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Digging.Grid
{
    public class GridController
    {
        private readonly GridSettings settings;
        private readonly VisualGridSeparation separation;
        private readonly GridVisualizationForGarden visualizationForGarden;
        private readonly GridSpawner spawner;

        private List<GameObject> gardenTiles;

        private GridController(GridSettings settings,
                             GridSpawner spawner,
                             VisualGridSeparation separation,
                             GridVisualizationForGarden visualizationForGarden,
                             GameStateController gameStateController)
        {
            this.settings = settings;
            this.separation = separation;
            this.visualizationForGarden = visualizationForGarden;
            this.spawner = spawner;

            spawner.CreateGrid();
            spawner.Extented += OnExtented;
            gameStateController.GameState.Subscribe(OnChangedGameState);
        }

        private void OnExtented()
        {
            separation.Draw(spawner.Parents);
        }
        
        public void Cancel()
        {
            foreach (var tile in gardenTiles)
            {
                Object.Destroy(tile);
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
                    var tile = Object.Instantiate(settings.TilePref);
                    tile.SetActive(false);
                    gardenTiles.Add(tile);
                }
            }
        }

        public bool CanPlace(Vector2Int position, Vector2Int size)
        {
            var currTiles = spawner.Tiles.GetTilesAround(position, size);
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
            var tilesAround = spawner.Tiles.GetTilesAround(position, size);
            tilesAround.ForEach(tile => tile.SetBuilding(building));
            return tilesAround;
        }

        public void HighlightTiles(Vector2Int size, Vector2Int position)
        {
            visualizationForGarden.SetTilesPosition(spawner.Tiles.GetTilesAround(position, size), gardenTiles);
            visualizationForGarden.PaintTiles(spawner.Tiles.GetTilesAround(position, size), gardenTiles, settings);
        }

        public bool TryGetTileOnMouse(out Tile tile)
        {
            tile = null;
            var position = GridRaycaster.GetRaycastIntPosition();
            if (position.x >= spawner.Tiles.MinX && position.x < spawner.Tiles.MaxX
             && position.y >= spawner.Tiles.MinY && position.y < spawner.Tiles.MaxY)
            {
                tile = spawner.Tiles.GetTile(position.x, position.y);
                return true;
            }
            return false;
        }

        private void OnChangedGameState(GameStates currentState)
        {
            switch (currentState)
            {
                case GameStates.Game:
                    separation.UnDraw(spawner.Parents);
                    break;
                case GameStates.Redactor or GameStates.Building or GameStates.Expansion:
                    separation.Draw(spawner.Parents);
                    break;
            }
        }
    }
}