using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFeaturePlacer : MonoBehaviour {

    private static TerrainFeaturePlacer instance = null;
    public static TerrainFeaturePlacer Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public Map map;
    public GameObject forestPrefab;

    public GameObject featuresBucket;

    public void PlaceFeature(GameObject feature, int distanceFromSameFeature, List<TileTerrain> allowedTerrain)
    {
        List<GameTile> placed = new List<GameTile>();
        List<GameTile> allowedTiles = new List<GameTile>();

        if (allowedTerrain.Contains(TileTerrain.DeepSea))
        {
            allowedTiles.AddRange(map.regionDeepSea);
        }
        if (allowedTerrain.Contains(TileTerrain.ShallowSea))
        {
            allowedTiles.AddRange(map.regionSea);
        }
        if (allowedTerrain.Contains(TileTerrain.Sand) || allowedTerrain.Contains(TileTerrain.Land))
        {
            allowedTiles.AddRange(map.regionSand);
        }
        if (allowedTerrain.Contains(TileTerrain.Grass) || allowedTerrain.Contains(TileTerrain.Grass2) || allowedTerrain.Contains(TileTerrain.Land))
        {
            allowedTiles.AddRange(map.regionGrass);
        }
        if (allowedTerrain.Contains(TileTerrain.Mountain) || allowedTerrain.Contains(TileTerrain.Mountain2) || allowedTerrain.Contains(TileTerrain.Land))
        {
            allowedTiles.AddRange(map.regionMountain);
        }
        if (allowedTerrain.Contains(TileTerrain.Snow) || allowedTerrain.Contains(TileTerrain.Snow2) || allowedTerrain.Contains(TileTerrain.Land))
        {
            allowedTiles.AddRange(map.regionSnow);
        }

        Debug.Log("wat " + allowedTiles.Count);
        for (int i = 0; i < allowedTiles.Count; i++)
        {
            if (EvaluateFeaturePosition(allowedTiles[i].transform.position, placed, distanceFromSameFeature))
            {
                GameObject temp = Instantiate(feature);
                temp.transform.localPosition = allowedTiles[i].transform.position;
                temp.transform.parent = featuresBucket.transform;
                placed.Add(allowedTiles[i]);
                allowedTiles[i].tileFeatures = TileFeatures.Forest;
            }
        }

    }

    private bool EvaluateFeaturePosition(Vector3 pos, List<GameTile> placedList, int distance)
    {
        for (int i = 0; i < placedList.Count; i++)
        {

            if (Mathf.Abs(placedList[i].transform.position.x - pos.x) <= 2 && Mathf.Abs(placedList[i].transform.position.z - pos.z) <= 2)
            {
                return false;
            }
        }
        return true;
    }
}
