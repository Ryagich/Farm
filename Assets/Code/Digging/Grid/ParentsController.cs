using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Digging.Grid
{
    public class ParentsController
    {
        public List<GridParent> Parents { get; private set; } = new();

        private readonly GridSettings settings;

        public ParentsController(GridSettings settings)
        {
            this.settings = settings;
            Parents = CreateParents();
            MergeParents(Parents);
        }

        private List<GridParent> CreateParents()
        {
            var result = new List<GridParent>();
            foreach (var info in settings.Info)
            {
                var parent = Object.Instantiate(settings.Parent);
                parent.transform.position = new Vector3(info.Position.x, 0, info.Position.y);
                parent.Initialize(new Vector2Int(info.Position.x, info.Position.y),
                                  new Vector2Int(info.Size.x, info.Size.y),
                                  info.Type);
                result.Add(parent);
            }
            return result;
        }

        public void MergeParents(List<GridParent> parents)
        {
            var i = 0;
            while (i < parents.Count)
            {
                var startParentsCount = parents.Count;
                for (var j = 0; j < parents.Count; j++)
                {
                    if (startParentsCount != parents.Count
                     || parents[i] == parents[j])
                        continue;
                    if (parents[i].Type == parents[j].Type
                     && TryMerge(parents[i], parents[j], out var newParent))
                    {
                        var a = parents[i];
                        var b = parents[j];
                        parents[i] = newParent;
                        parents.Remove(b);
                        Object.Destroy(a.gameObject);
                        Object.Destroy(b.gameObject);
                        break;
                    }
                }
                if (startParentsCount == parents.Count)
                {
                    i++;
                }
            }
        }

        private bool TryMerge(GridParent a, GridParent b, out GridParent result)
        {
            result = null;
            var canMergeHorizontally = a.Position.y == b.Position.y && a.Size.y == b.Size.y
                                      && (a.Position.x + a.Size.x == b.Position.x ||
                                                                        b.Position.x + b.Size.x == a.Position.x);
            var canMergeVertically = a.Position.x == b.Position.x && a.Size.x == b.Size.x
                                                                  && (a.Position.y + a.Size.y == b.Position.y ||
                                                                      b.Position.y + b.Size.y == a.Position.y);
            if (canMergeHorizontally || canMergeVertically)
            {
                var x = Math.Min(a.Position.x, b.Position.x);
                var y = Math.Min(a.Position.y, b.Position.y);

                result = Object.Instantiate(settings.Parent);
                result.Initialize(new Vector2Int(x, y),
                                  new Vector2Int(canMergeHorizontally ? a.Size.x + b.Size.x : a.Size.x,
                                                 canMergeVertically ? a.Size.y + b.Size.y : a.Size.y),
                                  a.Type);
                result.transform.position = new Vector3(x, 0, y);
                return true;
            }
            return false;
        }
    }
}