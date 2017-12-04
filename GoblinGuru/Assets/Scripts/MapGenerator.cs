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

    public TerrainType[] regions;

    float[,] fallOffMap;

    public GameObject gameTileBucket;

    public GameTile tilePrefab;

    private GameTile[] gameTiles = new GameTile[mapChunkSize * mapChunkSize];

    private void Awake()
    {
        fallOffMap = FallOffGenerator.GenerateFallOffMap(mapChunkSize);
    }

    private void Start()
    {
        gameTiles = new GameTile[mapChunkSize * mapChunkSize];
    }

    public void GenerateMap(){

        float[,] noiseMap = NoiseGenerator.GenerateNoise(mapChunkSize, mapChunkSize, seed,  noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

        for (int x = 0; x < mapChunkSize; x++){
            for (int y = 0; y < mapChunkSize; y++){

                if (useFallOff)
                {
                    noiseMap[x, y] = Mathf.Clamp01( noiseMap[x, y] - fallOffMap[x, y] );
                }

                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++){
                    if (currentHeight <= regions[i].height){
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        RiverReturn river = RiverGenerator.GenerateRivers(noiseMap);

        //noiseMap = river.noiseMap;

        int t;
        for (t = 0; t < river.riverPoints.Length; t++)
        {
            int x = (int)river.riverPoints[t].x;
            int y = (int)river.riverPoints[t].y;
            //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(y,5,x), Quaternion.identity);
            colorMap[y * mapChunkSize + x] = new Color(34 / 255f, 51 / 255f, 219 / 255f);
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        if(drawmode == DrawMode.noiseMap){
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawmode == DrawMode.ColorMap){
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawmode == DrawMode.Mesh){
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail);
            mapDisplay.DrawMesh(meshData, TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
            GenerateGameTiles(meshData, mapChunkSize, mapChunkSize );
        }
        else if (drawmode == DrawMode.FallOffMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(FallOffGenerator.GenerateFallOffMap(mapChunkSize)));
        }
    }

    public GameTile[] GenerateGameTiles(MeshData meshData, int width, int height )
    {
        ClearGameTiles();

        if (Application.isPlaying)
        {
            for (int x = 0; x < width-1; x++)
            {
                for (int y = 0; y < height-1; y++)
                {
                    GameTile tile = Instantiate(tilePrefab, meshData.tilePosition[x * mapChunkSize + y], Quaternion.identity);
                    gameTiles[x * mapChunkSize + y] = tile;
                    tile.Initialize(gameTileBucket.transform, x, y);
                    tile.SetNeighbours(null, (y != 0) ? gameTiles[x * mapChunkSize + y - 1 ] : null,
                        (x != 0) ? gameTiles[(x - 1) * mapChunkSize + y] : null, null);
                }
            }
        }

        return gameTiles;
    }

    public void ClearGameTiles()
    {
        for (int i = 0; i < gameTiles.Length; i++)
        {
            if (gameTiles[i] != null)
            {
                gameTiles[i].Destroy();
            }
        }

        gameTiles = new GameTile[mapChunkSize * mapChunkSize];
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

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
