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

    public GameTile tilePrefab;

    private GameTile[] gameTiles = new GameTile[mapChunkSize * mapChunkSize];

    private void Awake()
    {
        fallOffMap = FallOffGenerator.generateFallOffMap(mapChunkSize);
    }

    public void generateMap(){

        for (int i = 0; i < gameTiles.Length; i++)
        {
            if(gameTiles[i] != null)
            {
                gameTiles[i].Destroy();
            }
        }
        
        gameTiles = new GameTile[mapChunkSize * mapChunkSize];
        float[,] noiseMap = NoiseGenerator.generateNoise(mapChunkSize, mapChunkSize, seed,  noiseScale, octaves, persistance, lacunarity, offset);
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

        RiverReturn river = RiverGenerator.generateRivers(noiseMap);

        //noiseMap = river.noiseMap;

        int t;
        for (t = 0; t < river.riverPoints.Length; t++)
        {
            int x = (int)river.riverPoints[t].x;
            int y = (int)river.riverPoints[t].y;
            //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(y,5,x), Quaternion.identity);
            colorMap[y * mapChunkSize + x] = new Color(34 / 255f, 51 / 255f, 219 / 255f);
        }
        Debug.Log("points: " + t);

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
            gameTiles = generateGameTiles(meshData, mapChunkSize, mapChunkSize );
        }
        else if (drawmode == DrawMode.FallOffMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(FallOffGenerator.generateFallOffMap(mapChunkSize)));
        }
    }

    public GameTile[] generateGameTiles(MeshData meshData, int width, int height )
    {
        GameTile[] gameTile = new GameTile[width * height];
        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                GameTile tile = Instantiate(tilePrefab, meshData.tilePosition[x * mapChunkSize + y], Quaternion.identity);
                gameTile[x * mapChunkSize + y] = tile;
                tile.transform.parent = transform.gameObject.transform;
                tile.name = x + ", " + y;

                Ray ray = new Ray(gameTile[x * mapChunkSize + y].transform.position, new Vector3(0, -1, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    gameTile[x * mapChunkSize + y].transform.position = hit.point;
                }

            }
        }

        return gameTile;
    }

    private void OnValidate(){

        if(lacunarity < 1){
            lacunarity = 1;
        }

        if(octaves < 0){
            octaves = 0;
        }

        fallOffMap = FallOffGenerator.generateFallOffMap(mapChunkSize);

    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
