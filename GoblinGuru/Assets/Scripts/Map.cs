using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Map : MonoBehaviour
{
    Mesh mapMesh;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    GameTile[] gameTiles;

    private float[,] heightMap;
    float heightMultiplier;
    AnimationCurve heightCurve;

    int levelOfDetail;

    int triangleIndex;
    int vertexIndex;

    int mapMeshWidth;
    int mapMeshHeight;

    public GameObject gameTileParent;
    public GameTile tilePrefab;

    public UnityEngine.Canvas mapCanvas;
    GameTileLabel[] labels;

    TileTerrain[] tileTerrain;

    public Vector3[] Vertices
    {
        get
        {
            return vertices;
        }
    }

    public int[] Triangles
    {
        get
        {
            return triangles;
        }

    }

    public Vector2[] Uvs
    {
        get
        {
            return uvs;
        }

    }

    public GameTile[] GameTiles
    {
        get
        {
            return gameTiles;
        }
    }

    public GameTile GetTile(int x, int y)
    {
        if(GameTiles != null)
        {
            return GameTiles[x * mapMeshWidth + y];
        }
        Debug.Log("null!!");
        return null;
    }

    private void CalculateMeshFromHeightMap()
    {
        float topLeftX = (mapMeshWidth - 1) / -2f;
        float topLeftZ = (mapMeshHeight - 1) / 2f;

        int meshSimplificationIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        int verticesPerLine = (mapMeshWidth - 1) / meshSimplificationIncrement + 1;

        vertexIndex = 0;
        triangleIndex = 0;

        for (int y = 0; y < mapMeshWidth; y += 1)
        {
            for (int x = 0; x < mapMeshHeight; x += 1)
            {
                Vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                Uvs[vertexIndex] = new Vector2(x / (float)mapMeshWidth, y / (float)mapMeshHeight);

                if (x < mapMeshWidth - 1 && y < mapMeshHeight - 1)
                {
                   AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                   AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
    }

    public GameTile[] GenerateGameTiles()
    {
        ClearGameTiles();

        if (Application.isPlaying)
        {
            for (int x = 0; x < mapMeshWidth -1; x++)
            {
                for (int y = 0; y < mapMeshHeight -1; y++)
                {
                    GameTile tile = Instantiate(tilePrefab, new Vector3(x + .5f, 10, y + .5f), Quaternion.identity);
                    GameTiles[x * mapMeshWidth + y] = tile;
                    tile.Initialize(gameTileParent.transform, x, y, tileTerrain[(x * (mapMeshWidth-1)) + y]);
                    tile.SetNeighbours(null, (y != 0) ? GameTiles[x * mapMeshWidth + y - 1] : null,
                        (x != 0) ? GameTiles[(x - 1) * mapMeshWidth + y] : null, null);
                }
            }
        }

        return GameTiles;
    }

    public void ClearGameTiles()
    {
        if(GameTiles != null)
        {
            for (int i = 0; i < GameTiles.Length; i++)
            {
                if (GameTiles[i] != null)
                {
                    GameTiles[i].Destroy();
                }
                if (labels[i] != null)
                {
                    labels[i].Destroy();
                }
            }
        }

        labels = new GameTileLabel[mapMeshWidth * mapMeshHeight];
        gameTiles = new GameTile[mapMeshWidth* mapMeshHeight];
    }

    public void Initialize(float[,] hM, float hMult, AnimationCurve hCurve, int lod, TileTerrain[] tT)
    {
        heightMap = hM;
        mapMeshWidth = hM.GetLength(0);
        mapMeshHeight = hM.GetLength(1);

        heightMultiplier = hMult;
        heightCurve = hCurve;
        levelOfDetail = lod;

        tileTerrain = tT;

        Clear();
    }

    private void Clear()
    {
        GetComponent<MeshFilter>().sharedMesh = mapMesh = new Mesh();
        mapMesh.name = "Map Mesh";

        vertices = new Vector3[mapMeshWidth * mapMeshHeight];
        uvs = new Vector2[mapMeshWidth * mapMeshHeight];
        triangles = new int[((mapMeshWidth - 1) * (mapMeshHeight - 1)) * 6];

        ClearGameTiles();
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }


    public Map BuildMesh()
    {
        Clear();
        CalculateMeshFromHeightMap();
        GetComponent<MeshFilter>().sharedMesh = mapMesh = new Mesh()
        {
            vertices = Vertices,
            triangles = Triangles,
            uv = Uvs
        };
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mapMesh;
        mapMesh.RecalculateNormals();
        return this;
    }

    public void ApplyTexture(Texture2D texture)
    {
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
    }
}