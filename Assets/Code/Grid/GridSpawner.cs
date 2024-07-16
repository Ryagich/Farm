using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Grid
{
    public class GridSpawner : MonoBehaviour
    {
        public static GridSpawner Instance;
        
        public Tile[,] Tiles { get; private set; }
        public List<GridParent> Parents { get; private set; } = new();
        
        [SerializeField] private UnityEvent<List<Tile>> _created;
        [SerializeField] private GridSettings _settings;
        
        private List<GameObject> planes = new();

        private void Awake()
        {
            Instance = this;

            SetTilesSize();
            CreateGrid();
        }

        [Button("Clear")]
        private void Clear()
        {
            foreach (var plane in planes)
            {
                Destroy(plane);
            }
            planes.Clear();
            foreach (var p in Parents)
            {
                Destroy(p.gameObject);
            }
            Tiles = new Tile[0, 0];
            Parents.Clear();
        }

        [Button("Set Tiles Size")]
        private void SetTilesSize()
        {
            var startPosition = _settings.Info[0].Position;
            var endPosition = _settings.Info[^1].Position;
            var endSize = _settings.Info[^1].Size;
            Tiles = new Tile[endPosition.x - startPosition.x + endSize.x,
                             endPosition.z - startPosition.z + endSize.y];
        }

        [Button("Create")]
        private void CreateGrid()
        {
            var startPosition = _settings.Info[0].Position;

            foreach (var info in _settings.Info)
            {
                var parent = Instantiate(_settings.Parent);
                var localPosition = startPosition;
                
                var plane = Instantiate(_settings.TilePref);
                var gridPlaneTransform = plane.transform;
                gridPlaneTransform.position = info.Position;
                gridPlaneTransform.localScale = new Vector3(-info.Size.x, gridPlaneTransform.localScale.y, info.Size.y);
                
                parent.transform.position = info.Position;
                parent.SetSize(new Vector2Int(info.Size.x, info.Size.y));
                parent.SetType(info.Type);
                
                for (var x = 0; x < info.Size.x; x++)
                {
                    for (var y = 0; y < info.Size.y; y++)
                    {
                        var index = new Vector2Int(x + localPosition.x + info.Position.x, y + localPosition.z + +info.Position.z);
                        var tile = new Tile(index, info.Type, TileState.Clear);
                        if (Tiles[tile.Index.x, tile.Index.y] != null)
                        {
                            throw new ArgumentOutOfRangeException("На этом месте уже есть тайл. Перепроверь позиции гридов");
                        } 
                        Tiles[tile.Index.x, tile.Index.y] = tile;
                    }
                }
                
                planes.Add(plane);
                Parents.Add(parent);
            }
        }
    }
}