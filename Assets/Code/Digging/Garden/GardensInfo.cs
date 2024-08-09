using System.Collections.Generic;
using Code.Garden;
using UnityEngine;

namespace Code.Digging.Garden
{
    [CreateAssetMenu(fileName = "GardensInfo", menuName = "GardensInfo")]
    public class GardensInfo : ScriptableObject
    {
        [field: SerializeField] public List<GardenInfo> Info { get; private set; }
        [field: SerializeField] public Material Ghost_Material { get; private set; }
        [field: SerializeField] public Material RedGhost_Material { get; private set; }
        [field: SerializeField] public Material Garden_Material { get; private set; }
        [field: SerializeField] public Building.Building BuildingPref { get; private set; }

        private void OnValidate()
        {
            Info.Sort( (a, b) => a.Size.x != b.Size.x
                ? a.Size.x.CompareTo(b.Size.x)
                : a.Size.y.CompareTo(b.Size.y));
        }
    }
}