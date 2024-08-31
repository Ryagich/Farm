using System.Collections.Generic;
using UnityEngine;

namespace Code.Digging.Grid
{
    public class PlanesController
    {
        public List<GameObject> Planes { get; private set; } = new();
        
        private readonly GridSettings settings;

        public PlanesController(GridSettings settings)
        {
            this.settings = settings;
        }
        
        public void UpdatePlanes(List<GridParent> parents)
        {
            foreach (var plane in Planes)
            {
                Object.Destroy(plane.gameObject);
            }
            Planes.Clear();
            foreach (var parent in parents)
            {
                var plane = Object.Instantiate(settings.TilePref);
                var gridPlaneTransform = plane.transform;
                gridPlaneTransform.position = new Vector3(parent.Position.x, 0, parent.Position.y);
                gridPlaneTransform.localScale = new Vector3(-parent.Size.x, gridPlaneTransform.localScale.y, parent.Size.y);
                Planes.Add(plane);
            }
        }
    }
}