using System.Collections.Generic;
using Code.Game;
using Code.Input;
using Code.Materials;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Code.Digging.Grid.Extension
{
    public class ExtensionFounder : MonoBehaviour
    {
        private GameStateController gameStateController;
        private GridSettings gridSettings;
        private InputKeys keys;
        private MaterialsConfig materials;
        private LayersConfig layers;
        private GridExtension gridExtension;
        private GridSpawner spawner;
        
        private List<GameObject> tiles = new();
        private ExtensionPointer currentPointer;

        [Inject]
        private void Constructor(GameStateController gameStateController,
                                 GridSettings gridSettings,
                                 InputKeys keys,
                                 MaterialsConfig materials,
                                 LayersConfig layers,
                                 GridExtension gridExtension,
                                 GridSpawner spawner)
        {
            this.gameStateController = gameStateController;
            this.gridSettings = gridSettings;
            this.keys = keys;
            this.materials = materials;
            this.layers = layers;
            this.gridExtension = gridExtension;
            this.spawner = spawner;
            
            keys.LeftMouse.action.canceled += ExtentGrid;

            gameStateController.GameState.Subscribe(value =>
                                                    {
                                                        if (value != GameStates.Expansion)
                                                        {
                                                            Hide();
                                                        }
                                                    });
        }

        private void ExtentGrid(InputAction.CallbackContext context)
        {
            if (currentPointer != null)
            {
                spawner.Extent(currentPointer);
                Debug.Log($"ExtentGrid");
            }
        }

        public void Update()
        {
            if (gameStateController.GameState.Value == GameStates.Expansion)
                if (TryGetRaycastExtension(out var pointer))
                {
                    if (pointer != currentPointer)
                    {
                        Hide();
                        currentPointer = pointer;
                        GetTiles(pointer);
                        for (var i = 0; i < pointer.Tiles.Count; i++)
                        {
                            tiles[i].SetActive(true);
                            Debug.Log($"Direction {pointer.Direction}");
                            tiles[i].transform.position = new Vector3(pointer.Tiles[i].Index.x + pointer.Direction.x + 1,
                                                                      0,
                                                                      pointer.Tiles[i].Index.y + pointer.Direction.y);
                            tiles[i].GetComponent<MeshRenderer>().material = materials.Ghost_Material;
                        }
                    }
                }
                else
                    Hide();
        }

        private void Hide()
        {
            currentPointer = null;
            foreach (var tile in tiles)
                tile.SetActive(false);
        }

        private void GetTiles(ExtensionPointer pointer)
        {
            while (tiles.Count < pointer.Tiles.Count)
            {
                var tile = Instantiate(gridSettings.TilePref);
                tiles.Add(tile);
            }
        }

        private bool TryGetRaycastExtension(out ExtensionPointer pointer)
        {
            pointer = null;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, 100,layers.Extension_Layer))
            {
                if (hit.transform != null)
                {
                    pointer = hit.transform.GetComponent<ExtensionPointer>();
                    return pointer != null;
                }
            }
            return false;
        }
    }
}