using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Grid
{
    public class GridUpper : MonoBehaviour
    {
        [SerializeField] private GridSettings _settings;
        [SerializeField] private InputActionReference _input;
        [SerializeField] private float _power = .1f;
        [SerializeField] private Vector2Int _size = new(2, 2);

        private Coroutine coroutine;
        private List<GameObject> tilesGo = new();

        private void Awake()
        {
            _input.action.performed += OnTryMove;
            _input.action.canceled += OnTryMove;
        }

        private void OnTryMove(InputAction.CallbackContext context)
        {
            if (GameStateController.Instance.GameState == GameStates.Game)
            {
                return;
            }
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    for (var x = 0; x < _size.x; x++)
                    {
                        for (var y = 0; y < _size.y; y++)
                        {
                            var tile = Instantiate(_settings.TilePref);
                            tile.SetActive(false);
                            tilesGo.Add(tile);
                        }
                    }
                    coroutine = StartCoroutine(UpCor(_size));
                    break;
                case InputActionPhase.Canceled:
                    foreach (var tile in tilesGo)
                    {
                        Destroy(tile);
                    }
                    tilesGo.Clear();
                    StopCoroutine(coroutine);
                    break;
            }
        }

        private IEnumerator UpCor(Vector2Int size)
        {
            while (true)
            {
                if (TryGetRaycastPosition(out var position))
                {
                    UpTiles(GetTilesAround(position));
                }
                yield return null;
            }
        }

        private List<Tile> GetTilesAround(Vector2Int position)
        {
            var result = new List<Tile>();
            var gridTiles = GridSpawner.Instance.Tiles;
            if (position.x < 0 || position.x >= gridTiles.GetLength(0)
             || position.y < 0 || position.y >= gridTiles.GetLength(1))
            {
                return result;
            }
            for (var x = position.x - 1; x < position.x + 1; x++)
            {
                if (x < 0 || x >= gridTiles.GetLength(0))
                {
                    continue;
                }
                for (var y = position.y - 1; y < position.y + 1; y++)
                {
                    if (y < 0 || y >= gridTiles.GetLength(1))
                    {
                        continue;
                    }
                    if (gridTiles[x, y] != null)
                    {
                        result.Add(gridTiles[x, y]);
                    }
                }
            }
            return result;
        }

        private void UpTiles(List<Tile> tiles)
        {
            foreach (var tile in tilesGo)
            {
                tile.SetActive(false);
            }
            //Debug.Log($"tiles.Count: {tiles.Count}");

            for (var i = 0; i < tiles.Count; i++)
            {
                tilesGo[i].SetActive(true);
                var position = new Vector3(tiles[i].Index.x + 1,0,tiles[i].Index.y);
                position = position.WithY(position.y + .1f);
                tilesGo[i].transform.position = position;
            }
        }

        private bool TryGetRaycastPosition(out Vector2Int position)
        {
            position = Vector2Int.zero;
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out var enter))
            {
                var hitPoint = ray.GetPoint(enter);
                hitPoint = hitPoint.WithZ(hitPoint.z < 0 ? -1: hitPoint.z);
                Debug.Log($"hitPoint: {hitPoint}");
                position = new Vector2Int((int)hitPoint.x, (int)hitPoint.z);
                Debug.Log($"position: {position}");
                return true;
            }
            if (Physics.Raycast(ray, out var hit, 100))
            {
                var pos3 = hit.transform.position;
                position = new Vector2Int((int)pos3.x, (int)pos3.z);
                Debug.Log($"position: {position}");
                return true;
                // var tile = hit.transform.GetComponent<Tile>();
                // return tile;
            }
            return false;
        }
    }
}