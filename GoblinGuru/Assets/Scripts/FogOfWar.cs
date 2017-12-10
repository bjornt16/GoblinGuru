using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {

    private static FogOfWar instance = null;
    public static FogOfWar Instance { get { return instance; } }

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

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    private Color[] colorMap;
    private int width;
    private int height;

    public void Initialize(Vector3[] vert, int[] tri, Vector2[] uv, int w, int h)
    {
        width = w;
        height = h;
        colorMap = GenerateBlankColorMap();
        meshFilter.sharedMesh = new Mesh()
        {
            vertices = vert,
            triangles = tri,
            uv = uv
        };

        Vector3 p = meshFilter.transform.localPosition;
        p.y += 1;
        meshFilter.transform.SetPositionAndRotation(p, meshFilter.transform.rotation);

        meshRenderer.sharedMaterial.mainTexture = TextureGenerator.TextureFromColorMap(colorMap, width, height);
        meshFilter.sharedMesh.RecalculateNormals();
    }

    public void ClearFog(GameTile tile, int range)
    {
        for (int x = (int)tile.coordinates.x - 2; x <= (int)tile.coordinates.x + 2; x++)
        {
            for (int y = (int)tile.coordinates.y - 2; y <= (int)tile.coordinates.y + 2; y++)
            {
                colorMap[x * width + y] = new Color(0, 0, 0, 0);
            }
        }
        meshRenderer.sharedMaterial.mainTexture = TextureGenerator.TextureFromColorMap(colorMap, width, height);
        meshFilter.sharedMesh.RecalculateNormals();
    }


    private Color[] GenerateBlankColorMap()
    {
        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = new Color(0, 0, 0, 255);
            }
        }

        return colorMap;
    }


}
