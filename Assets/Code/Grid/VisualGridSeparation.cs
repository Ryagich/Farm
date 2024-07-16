using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Code.Grid
{
    public class VisualGridSeparation : MonoBehaviour
    {
        [SerializeField] private GridSettings _settings;

        private void Start()
        {
            GameStateController.Instance.ChangedGameState.AddListener(OnChangedGameState);
        }

        private void OnChangedGameState(GameStates currentState)
        {
            switch (currentState)
            {
                case GameStates.Game:
                    UnDraw();
                    break;
                case GameStates.Redactor:
                    Draw();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentState));
            }
        }
        
        [Button("UnDraw")]
        private void UnDraw()
        {
            for (var i = 0; i < _settings.Info.Count; i++)
            {
                var line = GridSpawner.Instance.Parents[i].GetComponentInChildren<LineRenderer>();
                line.positionCount = 0;
            }
        }
        
        [Button("Draw")]
        private void Draw()
        {
            var scale = _settings.TilePref.transform.localScale;
            for (var i = 0; i < _settings.Info.Count; i++)
            {
                var points = new List<Vector3>();
                var info = _settings.Info[i];
                for (var z = 0; z <= info.Size.y; z++)
                {
                    var offset = scale.z * z;
                    var p1 = new Vector3(0, -offset, _settings.yOffset);
                    var p2 = new Vector3(info.Size.x * scale.z, -offset, _settings.yOffset);
                    if (z % 2 == 0)
                    {
                        (p1, p2) = (p2, p1);
                    }
                    points.Add(p1);
                    points.Add(p2);
                }
        
                for (var x = info.Size.x; x >= 0; x--)
                {
                    var offset = scale.x * x;
                    var p1 = new Vector3(offset, 0, _settings.yOffset);
                    var p2 = new Vector3(offset, -info.Size.y * scale.x, _settings.yOffset);
                    if (x % 2 == 0)
                    {
                        (p1, p2) = (p2, p1);
                    }
                    points.Add(p1);
                    points.Add(p2);
                }
                var line = GridSpawner.Instance.Parents[i].GetComponentInChildren<LineRenderer>();
                line.positionCount = points.Count;
                line.SetPositions(points.ToArray());
            }
        }
    }
}