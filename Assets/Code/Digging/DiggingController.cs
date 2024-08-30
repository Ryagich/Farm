using System.Collections;
using System.Collections.Generic;
using Code.Digging.Garden;
using Code.Digging.Grid;
using Code.Digging.Grid.Utils;
using Code.Game;
using Code.Grid;
using Code.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Code.Digging
{
    public class DiggingController : MonoBehaviour
    {
        private InputKeys keys;
        private GridController gridController;
        private GardenSpawner gardenSpawner;
        private GameStateController gameStateC;

        private Coroutine coroutine;
        private InputHandler inputHandler;
        
        [Inject]
        private void Constructor(InputKeys keys,
                                 InputHandler inputHandler,
                                 GridController gridController,
                                 GardenSpawner gardenSpawner,
                                 GameStateController gameStateC)
        {
            this.keys = keys;
            this.inputHandler = inputHandler;
            this.gridController = gridController;
            this.gardenSpawner = gardenSpawner;
            this.gameStateC = gameStateC;
        }

        private void Rotate(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled
             && inputHandler.ControlPressed)
            {
                var oldSize = gardenSpawner.Building.Size;
                var size = new Vector2Int(gardenSpawner.Building.Size.y, gardenSpawner.Building.Size.x);
                var tiles = gardenSpawner.Building.Tiles;
                var lastPos = gardenSpawner.Building.LastPosition == default
                                  ? gardenSpawner.Building.transform.position
                                  : gardenSpawner.Building.LastPosition;
                gardenSpawner.Destroy();
                gardenSpawner.Spawn(size, tiles, lastPos, oldSize);
            }
        }
        
        public void ShowGarden(GardenInfo info)
        {
            ShowGarden(info.Size);
        }
        
        public void ShowGarden(Vector2Int size, List<Tile> tiles = null,Vector3 lastPos = default)
        {
            if (coroutine != null)
            {
                var gardenTiles = gardenSpawner.Building.Tiles;
                if (gardenTiles?.Count > 0)
                {
                    gardenTiles.ForEach(tile => tile.SetBuilding(gardenSpawner.Building));
                    gardenSpawner.Building.transform.position = gardenSpawner.Building.LastPosition;
                    gardenSpawner.SetGarden(gardenTiles);
                }
                else
                {
                    gardenSpawner.Destroy();
                }
                Cancel();
            }
            gardenSpawner.Spawn(size, tiles, lastPos);
            gridController.SetGardenTiles(size);
            coroutine = StartCoroutine(MoveCor());
            
            keys.LeftMouse.action.canceled += OnSet;
            keys.RightMouse.action.canceled += OnCancel;
            keys.R.action.canceled += Rotate;
        }

        private IEnumerator MoveCor()
        {
            while (true)
            {
                var position = GridRaycaster.GetRaycastIntPosition();
                gridController.HighlightTiles(gardenSpawner.Building.Size, position);
                gardenSpawner.HighlightGarden(position, gridController.CanPlace(position, gardenSpawner.Building.Size));
                yield return null;
            }
        }
        
        private void OnSet(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled
                && coroutine != null
                && gridController.CanPlace(GridRaycaster.GetRaycastIntPosition(), gardenSpawner.Building.Size))
            {
                var tiles = gridController.ChangeGardenTilesState(gardenSpawner.Building.Size, gardenSpawner.Building);
                gardenSpawner.SetGarden(tiles);
                Cancel();
            }
        }
        
        private void OnCancel(InputAction.CallbackContext context)
        {
            if (gameStateC.GameState.Value == GameStates.Building
                && context.phase == InputActionPhase.Canceled
                && coroutine != null)
            {
                var tiles = gardenSpawner.Building.Tiles;
                if (tiles?.Count > 0)
                {
                    tiles.ForEach(tile => tile.SetBuilding(gardenSpawner.Building));
                    gardenSpawner.Building.transform.position = gardenSpawner.Building.LastPosition;
                    if (gardenSpawner.Building.LastSize != default)
                    {
                        gardenSpawner.Building.SetSize(gardenSpawner.Building.LastSize);
                    }
                    gardenSpawner.SetGarden(tiles);
                }
                else
                {
                    gardenSpawner.Destroy();
                }
                Cancel();
            }
        }
        
        private void Cancel()
        {
            keys.LeftMouse.action.canceled -= OnSet;
            keys.RightMouse.action.canceled -= OnCancel;
            keys.R.action.canceled -= Rotate;
            gridController.Cancel();
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}