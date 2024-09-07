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
        public Tiles Tiles => tilesController.Tiles; 
        public List<GridParent> Parents => parentsController.Parents; 
        public List<GameObject> Planes => planesController.Planes;
        
        private readonly GridSettings settings;
        private readonly VisualGridSeparation separation;
        private readonly GridVisualizationForGarden visualizationForGarden;
        
        private readonly TilesController tilesController;
        private readonly ParentsController parentsController;
        private readonly PlanesController planesController;
        
        private List<GameObject> gardenTiles;

        private GridController(GridSettings settings,
                               VisualGridSeparation separation,
                               GridVisualizationForGarden visualizationForGarden,
                               GameStateController gameStateController)
        {
            this.settings = settings;
            this.separation = separation;
            this.visualizationForGarden = visualizationForGarden;

            tilesController = new TilesController(settings);
            parentsController = new ParentsController(settings);
            planesController = new PlanesController(settings);
            planesController.UpdatePlanes(Parents);
            gameStateController.GameState.Subscribe(OnChangedGameState);
        }

        public void Extent(ExtensionPointer pointer)
        {
            tilesController.Extent(pointer, Tiles, Parents);
            parentsController.MergeParents(Parents);
            planesController.UpdatePlanes(Parents);

            separation.Draw(Parents);
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
            var currTiles = Tiles.GetTilesAround(position, size);
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

        public List<Tile> ChangeGardenTilesState(Building.Building building)
        {
            var position = GridRaycaster.GetRaycastIntPosition();
            var tilesAround = Tiles.GetTilesAround(position, building.Size);
            tilesAround.ForEach(tile => tile.SetBuilding(building));
            return tilesAround;
        }

        public void HighlightTiles(Vector2Int size, Vector2Int position)
        {
            visualizationForGarden.SetTilesPosition(Tiles.GetTilesAround(position, size), gardenTiles);
            visualizationForGarden.PaintTiles(Tiles.GetTilesAround(position, size), gardenTiles, settings);
        }

        public bool TryGetTileOnMouse(out Tile tile)
        {
            tile = null;
            var position = GridRaycaster.GetRaycastIntPosition();
            if (position.x >= Tiles.MinX && position.x < Tiles.MaxX
             && position.y >= Tiles.MinY && position.y < Tiles.MaxY)
            {
                tile = Tiles.GetTile(position.x, position.y);
                return true;
            }
            return false;
        }

        private void OnChangedGameState(GameStates currentState)
        {
            switch (currentState)
            {
                case GameStates.Game:
                    separation.UnDraw(Parents);
                    break;
                case GameStates.Redactor or GameStates.Building or GameStates.Expansion:
                    separation.Draw(Parents);
                    break;
            }
        }
    }
}