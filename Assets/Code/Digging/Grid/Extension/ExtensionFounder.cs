﻿using System.Collections.Generic;
using Code.Game;
using Code.Input;
using Code.Materials;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Code.Digging.Grid.Extension
{
    public class ExtensionFounder : ITickable
    {
        private readonly GameStateController gameStateController;
        private readonly GridSettings gridSettings;
        private readonly MaterialsConfig materials;
        private readonly LayersConfig layers;
        private readonly GridController gridController;

        private readonly List<GameObject> tiles = new();
        private readonly GridExtensionSpawner gridExtensionSpawner;

        private ExtensionPointer currentPointer;
        
        private ExtensionFounder(GameStateController gameStateController,
                                 GridSettings gridSettings,
                                 InputKeys keys,
                                 MaterialsConfig materials,
                                 LayersConfig layers,
                                 GridController gridController,
                                 GridExtensionSpawner gridExtensionSpawner)
        {
            this.gameStateController = gameStateController;
            this.gridSettings = gridSettings;
            this.materials = materials;
            this.layers = layers;
            this.gridController = gridController;
            this.gridExtensionSpawner = gridExtensionSpawner;
            
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
                gridController.Extent(currentPointer);
                gridExtensionSpawner.OnExtension();
            }
        }

        public void Tick()
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
                            tiles[i].transform.position =
                                new Vector3(pointer.Tiles[i].Index.x + pointer.Direction.x + 1,
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
                var tile = Object.Instantiate(gridSettings.TilePref);
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