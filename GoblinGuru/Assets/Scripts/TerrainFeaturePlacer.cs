using System;
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
    public GameObject forestSnowPrefab;
    public GameObject cityPrefab;
    public GameObject villagePrefab;
    public GameObject cavePrefab;
    public GameObject bridgePrefab;
    public GameObject castlePrefab;
    public GameObject marshPrefab;

    public GameObject featuresBucket;

    public void PlaceFeature(GameObject feature, TileFeatures featureType, int distanceFromSameFeature, List<TileTerrain> allowedTerrain)
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
            if (EvaluateFeaturePosition(allowedTiles[i].transform.position, placed, distanceFromSameFeature) && allowedTiles[i].tileFeatures == TileFeatures.None)
            {
                GameObject temp = Instantiate(feature);
                temp.transform.localPosition = allowedTiles[i].transform.position;
                temp.transform.parent = featuresBucket.transform;
                placed.Add(allowedTiles[i]);
                allowedTiles[i].tileFeatures = featureType;
            }
        }

    }

    private bool EvaluateFeaturePosition(Vector3 pos, List<GameTile> placedList, int distance)
    {
        for (int i = 0; i < placedList.Count; i++)
        {

            if (Mathf.Abs(placedList[i].transform.position.x - pos.x) <= distance && Mathf.Abs(placedList[i].transform.position.z - pos.z) <= distance)
            {
                return false;
            }
        }
        return true;
    }

    internal void PlaceFeatures()
    {
        List<TileTerrain> forest = new List<TileTerrain>();
        forest.Add(TileTerrain.Grass);
        forest.Add(TileTerrain.Mountain);
        PlaceFeature(forestPrefab, TileFeatures.Forest, 4, forest);
        PlaceFeature(forestPrefab, TileFeatures.Forest, 3, forest);

        List<TileTerrain> marsh = new List<TileTerrain>();
        marsh.Add(TileTerrain.Grass);
        marsh.Add(TileTerrain.Sand);
        PlaceFeature(marshPrefab, TileFeatures.swamp, 10, marsh);

        List<TileTerrain> snowTree = new List<TileTerrain>();
        snowTree.Add(TileTerrain.Snow);
        PlaceFeature(forestSnowPrefab, TileFeatures.Forest, 4, snowTree);
    }
}
