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

    System.Random rnd = new System.Random();

    public void PlaceFeature(GameObject feature, TileFeatures featureType, int distanceFromSameFeature, List<TileTerrain> allowedTerrain)
    {
        List<GameTile> placed = new List<GameTile>();
        List<GameTile> allowedTiles = new List<GameTile>();

        if (allowedTerrain.Contains(TileTerrain.River))
        {
            allowedTiles.AddRange(map.regionRiver);
        }
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
                if(featureType == TileFeatures.Bridge)
                {
                    if(allowedTiles[i].tileDown.tileTerrain != TileTerrain.River && allowedTiles[i].tileUp.tileTerrain != TileTerrain.River &&
                        allowedTiles[i].tileDown.tileTerrain != TileTerrain.ShallowSea && allowedTiles[i].tileUp.tileTerrain != TileTerrain.ShallowSea)
                    {
                        GameObject temp = Instantiate(feature);
                        temp.transform.localPosition = allowedTiles[i].transform.position;
                        Vector3 tempRot = temp.transform.localEulerAngles;
                        tempRot.y = 90;
                        temp.transform.localEulerAngles = tempRot;
                        temp.transform.parent = featuresBucket.transform;
                        placed.Add(allowedTiles[i]);
                        allowedTiles[i].tileFeatures = featureType;
                    }
                    else if(allowedTiles[i].tileLeft.tileTerrain != TileTerrain.River && allowedTiles[i].tileRight.tileTerrain != TileTerrain.River &&
                        allowedTiles[i].tileDown.tileTerrain != TileTerrain.ShallowSea && allowedTiles[i].tileUp.tileTerrain != TileTerrain.ShallowSea)
                    {
                        GameObject temp = Instantiate(feature);
                        temp.transform.localPosition = allowedTiles[i].transform.position;
                        temp.transform.parent = featuresBucket.transform;
                        placed.Add(allowedTiles[i]);
                        allowedTiles[i].tileFeatures = featureType;
                    }
                }
                else
                {
                    GameObject temp = Instantiate(feature);
                    temp.transform.localPosition = allowedTiles[i].transform.position;
                    temp.transform.parent = featuresBucket.transform;
                    placed.Add(allowedTiles[i]);
                    allowedTiles[i].tileFeatures = featureType;

                    if(featureType == TileFeatures.Castle || featureType == TileFeatures.Cave || featureType == TileFeatures.Town ||
                        featureType == TileFeatures.Village || featureType == TileFeatures.swamp)
                    {
                        Vector3 tempRot = temp.transform.localEulerAngles;
                        tempRot.y = rnd.Next(0, 4) * 90;
                        temp.transform.localEulerAngles = tempRot;
                    }
                }

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

        List<TileTerrain> bridge = new List<TileTerrain>();
        bridge.Add(TileTerrain.River);
        PlaceFeature(bridgePrefab, TileFeatures.Bridge, 15, bridge);

        List<TileTerrain> castle = new List<TileTerrain>();
        castle.Add(TileTerrain.Mountain);
        castle.Add(TileTerrain.Snow);
        PlaceFeature(castlePrefab, TileFeatures.Castle, 25, castle);

        List<TileTerrain> city = new List<TileTerrain>();
        city.Add(TileTerrain.Mountain);
        city.Add(TileTerrain.Grass);
        PlaceFeature(cityPrefab, TileFeatures.Town, 25, city);

        List<TileTerrain> village = new List<TileTerrain>();
        village.Add(TileTerrain.Sand);
        village.Add(TileTerrain.Grass);
        PlaceFeature(villagePrefab, TileFeatures.Village, 25, village);

        List<TileTerrain> cave = new List<TileTerrain>();
        cave.Add(TileTerrain.Snow);
        cave.Add(TileTerrain.Mountain);
        PlaceFeature(cavePrefab, TileFeatures.Cave, 20, cave);
    }
}
