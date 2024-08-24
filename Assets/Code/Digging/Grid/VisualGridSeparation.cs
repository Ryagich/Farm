using System;
using System.Collections.Generic;
using Code.Digging.Grid;
using Code.Game;
using UnityEngine;

namespace Code.Grid
{
    public class VisualGridSeparation
    {
        private readonly GridSettings settings;
        
        private VisualGridSeparation(GridSettings settings)
        {
            this.settings = settings;
        }
        
        public void Draw(List<GridParent> parents)
        {
            var scale = settings.TilePref.transform.localScale;
            for (var i = 0; i < parents.Count; i++)
            {
                var points = new List<Vector3>();
                for (var z = 0; z <= parents[i].Size.y; z++)
                {
                    var offset = scale.z * z;
                    var p1 = new Vector3(0, -offset, settings.yOffset);
                    var p2 = new Vector3(parents[i].Size.x * scale.z, -offset, settings.yOffset);
                    if (z % 2 == 0)
                    {
                        (p1, p2) = (p2, p1);
                    }
                    points.Add(p1);
                    points.Add(p2);
                }
        
                for (var x = parents[i].Size.x; x >= 0; x--)
                {
                    var offset = scale.x * x;
                    var p1 = new Vector3(offset, 0, settings.yOffset);
                    var p2 = new Vector3(offset, -parents[i].Size.y * scale.x, settings.yOffset);
                    if (x % 2 == 0)
                    {
                        (p1, p2) = (p2, p1);
                    }
                    points.Add(p1);
                    points.Add(p2);
                }
                var line = parents[i].GetComponentInChildren<LineRenderer>();
                line.positionCount = points.Count;
                line.SetPositions(points.ToArray());
            }
        }
        
        public void UnDraw(List<GridParent> parents)
        {
            for (var i = 0; i < parents.Count; i++)
            {
                var line = parents[i].GetComponentInChildren<LineRenderer>();
                line.positionCount = 0;
            }
        }
    }
}