using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Grid.Utils
{
    public static class GridRaycaster
    {
        public static Vector2Int GetRaycastPosition()
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var plane = new Plane(Vector3.up, Vector3.zero);
            plane.Raycast(ray, out var enter);
            var hitPoint = ray.GetPoint(enter);
            hitPoint = hitPoint.WithZ(hitPoint.z < 0 ? -1 : hitPoint.z);
            return new Vector2Int((int)hitPoint.x, (int)hitPoint.z);
        }
    }
}