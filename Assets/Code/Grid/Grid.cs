using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Grid
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private UnityEvent<List<GameObject>> _created;
        [SerializeField] public GameObject _tile;
        [SerializeField] private GameObject _parent;
        [SerializeField] private GridSettings _settings;

        private readonly List<GameObject> tiles = new();
        [HideInInspector] public List<GameObject> Parents { get; private set; } = new();

        private void Awake()
        {
            CreateGrid();
        }

        [Button("Clear")]
        private void Clear()
        {
            foreach (var tile in tiles)
            {
                Destroy(tile.gameObject);
            }
            foreach (var p in Parents)
            {
                Destroy(p.gameObject);
            }
            tiles.Clear();
            Parents.Clear();
        }

        [Button("Create")]
        private void CreateGrid()
        {
            var scale = _tile.transform.localScale;
            foreach (var info in _settings.Info)
            {
                var parent = Instantiate(_parent);
                parent.transform.position = info.StartPoint;
                Parents.Add(parent);
                for (var y = 0; y < info.Size.y; y++)
                {
                    for (var x = 0; x < info.Size.x; x++)
                    {
                        var tile = Instantiate(_tile, parent.transform);
                        tile.transform.localPosition = new Vector3(scale.x * x, 0, scale.z * y);
                        tiles.Add(tile);
                    }
                }
            }
            _created?.Invoke(tiles);
        }
    }
}