using System.Collections.Generic;
using Code.Digging.Grid;
using UnityEngine;

namespace Code.Digging.Garden
{
    public class GardenSpawner
    {
        public Building.Building Building { get; private set; }
        
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
            Building = MonoBehaviour.Instantiate(gardensInfo.BuildingPref);
            Building.SetSize(size);
        }
        
        public void SetGarden(List<Tile> tiles)
        {
            Building.Visual.GetComponent<MeshRenderer>().material = gardensInfo.Garden_Material;
            Building.SetTiles(tiles);
            Building = null;
        }
        
        public void Destroy()
        {
            MonoBehaviour.Destroy(Building.gameObject);
            Building = null;
        }
        
        public void VisualizationGarden(Vector2Int position, bool canPlace)
        {
            Building.transform.position = new Vector3(position.x + 1,settings.yOffset,position.y + 1);
            Building.Visual.GetComponent<MeshRenderer>().material = canPlace
                                                                ? gardensInfo.Ghost_Material
                                                                : gardensInfo.RedGhost_Material;
        }
    }
}