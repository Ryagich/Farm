using System.Collections;
using Code.Digging.Garden;
using Code.Game;
using Code.Grid;
using Code.Grid.Utils;
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
        private Vector2Int size;
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
            Debug.Log($"Rotate");
            Debug.Log($"Control Pressed is {inputHandler.ControlPressed}");

            if (context.phase == InputActionPhase.Canceled
                && inputHandler.ControlPressed)
            {
                size = new Vector2Int(size.y, size.x);
                gardenSpawner.Destroy();
                gardenSpawner.Spawn(size);
            }
        }
        
        public void ShowGarden(GardenInfo info)
        {
            size = info.Size;
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
                var position = GridRaycaster.GetRaycastPosition();
                gridMediator.VisualizationTiles(size, position);
                gardenSpawner.VisualizationGarden(position, gridMediator.CanPlace(size));
                yield return null;
            }
        }
        
        private void OnSet(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled
                && coroutine != null
                && gridMediator.CanPlace(size))
            {
                gridMediator.ChangeGardenTilesState(size);
                gardenSpawner.SetGarden();
                Cancel();
            }
        }
        
        private void OnCancel(InputAction.CallbackContext context)
        {
            if (gameStateC.GameState.Value == GameStates.Redactor
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