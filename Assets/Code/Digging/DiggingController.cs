using System.Collections;
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
        private GridMediator gridMediator;
        private GardenSpawner gardenSpawner;
        private GameStateController gameStateC;

        private Coroutine coroutine;
        private InputHandler inputHandler;
        
        [Inject]
        private void Constructor(InputKeys keys,
                                 InputHandler inputHandler,
                                 GridMediator gridMediator,
                                 GardenSpawner gardenSpawner,
                                 GameStateController gameStateC)
        {
            this.keys = keys;
            this.inputHandler = inputHandler;
            this.gridMediator = gridMediator;
            this.gardenSpawner = gardenSpawner;
            this.gameStateC = gameStateC;
        }

        private void Rotate(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled
                && inputHandler.ControlPressed)
            {
                var size = new Vector2Int(gardenSpawner.Building.Size.y, gardenSpawner.Building.Size.x);
                gardenSpawner.Destroy();
                gardenSpawner.Spawn(size);
            }
        }
        
        public void ShowGarden(GardenInfo info)
        {
            ShowGarden(info.Size);
        }
        
        public void ShowGarden(Vector2Int size)
        {
            if (coroutine != null)
            {
                Cancel();
                gardenSpawner.Destroy();
            }
            gardenSpawner.Spawn(size);
            gridMediator.SetGardenTiles(size);
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
                gridMediator.VisualizationTiles(gardenSpawner.Building.Size, position);
                gardenSpawner.VisualizationGarden(position, gridMediator.CanPlace(gardenSpawner.Building.Size));
                yield return null;
            }
        }
        
        private void OnSet(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled
                && coroutine != null
                && gridMediator.CanPlace(gardenSpawner.Building.Size))
            {
                var tiles = gridMediator.ChangeGardenTilesState(gardenSpawner.Building.Size, gardenSpawner.Building);
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
                Cancel();
                gardenSpawner.Destroy();
            }
        }
        
        private void Cancel()
        {
            keys.LeftMouse.action.canceled -= OnSet;
            keys.RightMouse.action.canceled -= OnCancel;
            keys.R.action.canceled -= Rotate;
            gridMediator.Cancel();
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}