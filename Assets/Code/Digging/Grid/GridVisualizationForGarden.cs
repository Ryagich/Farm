using System.Collections.Generic;
using Code.Grid;
using UnityEngine;

namespace Code.Digging.Grid
{
    public class GridVisualizationForGarden
    {
        public void SetTilesPosition(List<Tile> tiles, List<GameObject> gardenTiles)
        {
            foreach (var tile in gardenTiles)
            {
                tile.SetActive(false);
            }
            for (var i = 0; i < tiles.Count; i++)
            {
                gardenTiles[i].SetActive(true);
                var position = new Vector3(tiles[i].Index.x + 1, 0, tiles[i].Index.y);
                position = position.WithY(position.y + .1f);
                gardenTiles[i].transform.position = position;
            }
        }
        
        public void PaintTiles(List<Tile> tiles, List<GameObject> gardenTiles, GridSettings settings)
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                switch (tiles[i].State)
                {
                    case TileState.Clear:
                        gardenTiles[i].GetComponent<MeshRenderer>().material = settings.ClearMaterial;
                        break;
                    case TileState.Busy:
                        gardenTiles[i].GetComponent<MeshRenderer>().material = settings.BusyMaterial;
                        break;
                }
            }
        }
    }
}