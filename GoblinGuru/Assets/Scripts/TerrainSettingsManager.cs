using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSettingsManager : MonoBehaviour {

    private static TerrainSettingsManager instance = null;
    public static TerrainSettingsManager Instance { get { return instance; } }

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


    public TerrainType[] regions;

    public TerrainType GetTerrainType(float val)
    {
        for (int i = 0; i < regions.Length; i++)
        {
            if (val <= regions[i].height)
            {
                return regions[i];
            }
        }

        return regions[regions.Length-1];
    }

}

[System.Serializable]
public struct TerrainType
{
    public TileTerrain type;
    public float height;
    public Color color;
}

public enum TileTerrain{
    DeepSea, ShallowSea, Lake, River, Sand, Grass, Grass2, Mountain, Mountain2, Snow, Snow2, Land, Sea
}

public enum TileFeatures
{
    None, Forest, Cave, swamp, Town, Village, Camp, Harbor, Bridge
}