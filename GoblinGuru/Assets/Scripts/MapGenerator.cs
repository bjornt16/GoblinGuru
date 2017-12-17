using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {noiseMap, ColorMap, Mesh, FallOffMap};
    public DrawMode drawmode;

    const int mapChunkSize = 121;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool useFallOff;
    public bool autoUpdate;

    float[,] fallOffMap;

    public PlayerUnit player;

    public Map map;
    public FogOfWar fogofwar;

    private void Awake()
    {
        fallOffMap = FallOffGenerator.GenerateFallOffMap(mapChunkSize);
    }

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap(){

        if (!Application.isPlaying)
        {
            return;
        }

        float[,] noiseMap = NoiseGenerator.GenerateNoise(mapChunkSize, mapChunkSize, seed,  noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        TileTerrain[] terrainMap = new TileTerrain[(mapChunkSize-1) * (mapChunkSize - 1)];

    
        for (int x = 0; x < mapChunkSize; x++){
            for (int y = 0; y < mapChunkSize; y++){

                if (useFallOff)
                {
                    noiseMap[x, y] = Mathf.Clamp01( noiseMap[x, y] - fallOffMap[x, y] );
                }

                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < TerrainSettingsManager.Instance.regions.Length; i++){
                    if (currentHeight <= TerrainSettingsManager.Instance.regions[i].height && currentHeight <= 1){
                        colorMap[y * mapChunkSize + x] = TerrainSettingsManager.Instance.regions[i].color;
                        if(x < (mapChunkSize - 1) && y < (mapChunkSize - 1))
                            terrainMap[y * (mapChunkSize - 1) + x] = TerrainSettingsManager.Instance.regions[i].type;
                        break;
                    }
                }
            }
        }

        RiverReturn river = RiverGenerator.GenerateRivers(noiseMap);

        int t;
        for (t = 0; t < river.riverPoints.Length; t++)
        {
            int x = (int)river.riverPoints[t].x;
            int y = (int)river.riverPoints[t].y;
            //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(y,5,x), Quaternion.identity);
            colorMap[y * mapChunkSize + x] = TerrainSettingsManager.Instance.regions[9].color;
            terrainMap[y * (mapChunkSize - 1) + x] = TerrainSettingsManager.Instance.regions[9].type;
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        if(drawmode == DrawMode.noiseMap){
            mapDisplay.DrawTexture(TextureGenerator.NoiseTextureFromHeightMap(noiseMap));
        }
        else if (drawmode == DrawMode.ColorMap){
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawmode == DrawMode.Mesh){
            map.Initialize(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail, terrainMap);
            map.BuildMesh();
            map.GenerateGameTiles();
            map.ApplyTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
            player.Instantiate(map.GenerateGameTiles()[((mapChunkSize-1) * (mapChunkSize-1)) / 2]);
            fogofwar.Initialize(map.Vertices, map.Triangles, map.Uvs, noiseMap.GetLength(0), noiseMap.GetLength(1));
            fogofwar.ClearFog(map.GameTiles[((mapChunkSize - 1) * (mapChunkSize - 1)) / 2], 3);
            TerrainFeaturePlacer.Instance.PlaceFeatures();
        }
        else if (drawmode == DrawMode.FallOffMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.NoiseTextureFromHeightMap(FallOffGenerator.GenerateFallOffMap(mapChunkSize)));
        }
    }

    private void OnValidate(){

        if(lacunarity < 1){
            lacunarity = 1;
        }

        if(octaves < 0){
            octaves = 0;
        }

        fallOffMap = FallOffGenerator.GenerateFallOffMap(mapChunkSize);

    }
}
