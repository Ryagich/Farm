using Code.Digging;
using Code.Digging.Garden;
using Code.Digging.Grid;
using Code.Game;
using Code.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Code.Building
{
    public class BuildingMenu
    {
        private UIDocument screen;
        private GameStateController gameStateController;
        private BuildingMenuConfig config;
        private GridMediator gridMediator;
        private GridSpawner gridSpawner;
        private GardenSpawner gardenSpawner;
        private DiggingController diggingController;

        private VisualElement menu = null;

        private BuildingMenu(
                InputKeys keys,
                UIDocument screen,
                GameStateController gameStateController,
                BuildingMenuConfig config,
                GridMediator gridMediator,
                GardenSpawner gardenSpawner,
                DiggingController diggingController
            )
        {
            this.diggingController = diggingController;
            this.gardenSpawner = gardenSpawner;
            this.gridMediator = gridMediator;
            this.screen = screen;
            this.gameStateController = gameStateController;
            this.config = config;
            this.gridMediator = gridMediator;

            gameStateController.UIElementClicked += CloseMenu;
            keys.RightMouse.action.canceled += ShowMenu;
        }


        private void ShowMenu(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled
             && gameStateController.GameState.Value is GameStates.Building
             && gardenSpawner.Building == null
             && gridMediator.TryGetTileOnMouse(out var tile) && !tile.IsFree)
            {
                CloseMenu();

                menu = config.MenuTree.CloneTree().Q<VisualElement>();
                menu.style.position = Position.Absolute;
                var pos = Mouse.current.position.ReadValue();
                menu.style.left = pos.x;
                menu.style.bottom = pos.y;

                var container = menu.Q<VisualElement>("Container");
                var moveButton = config.ButtonTree.CloneTree().Q<VisualElement>();
                moveButton.Q<Label>("Text").text = config.MoveButtonName;
                moveButton.RegisterCallback<ClickEvent>(_ => Move(tile));

                container.Add(moveButton);

                var deleteButton = config.ButtonTree.CloneTree().Q<VisualElement>();
                deleteButton.Q<Label>("Text").text = config.DeleteButtonName;
                deleteButton.style.marginTop = 72;
                deleteButton.RegisterCallback<ClickEvent>(_ => Delete(tile));

                container.Add(deleteButton);

                screen.rootVisualElement.Add(menu);
            }
        }

        private void Delete(Tile tile)
        {
            MonoBehaviour.Destroy(tile.Building.gameObject);
            CloseMenu();
        }

        private void Move(Tile tile)
        {
            var building = tile.Building;
            var tiles = building.Tiles;
            tiles.ForEach(t => t.SetBuilding(null));
            diggingController.ShowGarden(building.Size, tiles, building.transform.position);
            MonoBehaviour.Destroy(building.gameObject);

            CloseMenu();
        }

        private void CloseMenu()
        {
            if (menu != null)
            {
                screen.rootVisualElement.Remove(menu);
                menu = null;
            }
        }
    }
}