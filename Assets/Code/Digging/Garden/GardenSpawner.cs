using Code.Digging.Grid;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Code.Digging.Garden
{
    public class GardenSpawner
    {
        private Garden garden;
        private GridSettings settings;
        private GardensInfo gardensInfo;

        private GardenSpawner(GridSettings settings,
                              GardensInfo gardensInfo)
        {
            this.gardensInfo = gardensInfo;
            this.settings = settings;
        }

        public void Spawn(Vector2Int size)
        {
            garden = MonoBehaviour.Instantiate(gardensInfo.GardenPref);
            var scale = garden.transform.localScale;
            garden.transform.localScale = new Vector3(scale.x * size.x, scale.z, scale.y * size.y);
        }
        
        public void SetGarden()
        {
            garden.Visual.GetComponent<MeshRenderer>().material = gardensInfo.Garden_Material;
            garden = null;
        }
        
        public void Destroy()
        {
            MonoBehaviour.Destroy(garden.gameObject);
            garden = null;
        }
        
        public void VisualizationGarden(Vector2Int position, bool canPlace)
        {
            garden.transform.position = new Vector3(position.x + 1,settings.yOffset,position.y + 1);
            garden.Visual.GetComponent<MeshRenderer>().material = canPlace
                                                                ? gardensInfo.Ghost_Material
                                                                : gardensInfo.RedGhost_Material;
        }
    }
}