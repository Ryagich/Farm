using System.Collections.Generic;
using Code.Digging.Grid;
using Code.Materials;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Code.Digging.Garden
{
    public class GardenSpawner
    {
        public Building.Building Building { get; private set; }
        
        private readonly GridSettings settings;
        private readonly GardensInfo gardensInfo;
        private readonly MaterialsConfig materials;
        
        private GardenSpawner(GridSettings settings,
                              GardensInfo gardensInfo,
                              MaterialsConfig materials)
        {
            this.gardensInfo = gardensInfo;
            this.settings = settings;
            this.materials = materials;
        }

        public void Spawn(Vector2Int size, List<Tile> tiles = null, Vector3 lastPos = default, Vector2Int oldSize = default)
        {
            Building = Object.Instantiate(gardensInfo.BuildingPref);
            Building.SetTiles(tiles);
            Building.SetSize(size);
            Building.LastPosition = lastPos;
            Building.LastSize = oldSize;
        }
        
        public void SetGarden(List<Tile> tiles)
        {
            Building.Visual.GetComponent<MeshRenderer>().material = materials.Garden_Material;
            Building.SetTiles(tiles);
            Building = null;
        }
        
        public void Destroy()
        {
            Object.Destroy(Building.gameObject);
            Building = null;
        }
        
        public void HighlightGarden(Vector2Int position, bool canPlace)
        {
            Building.transform.position = new Vector3(position.x + 1,settings.yOffset,position.y + 1);
            Building.Visual.GetComponent<MeshRenderer>().material = canPlace
                                                                  ? materials.Ghost_Material
                                                                  : materials.RedGhost_Material;
        }
    }
}