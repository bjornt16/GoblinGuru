using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class RiverGenerator{

    //private static System.Random rng = new System.Random(123);

    public static RiverReturn GenerateRivers(float[,] heightMap) {

        List<Vector2> riverStartList = new List<Vector2>();
        List<Vector2> riverPoints = new List<Vector2>();
        List<Vector2> riverPointsVect = new List<Vector2>();
        List<float> tempRiverPoints = new List<float>();
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sampleX = x;
                int sampleY = y;
                if (EvaluateRiverStartPosition(riverStartList,  new Vector2(sampleX, sampleY)) && heightMap[x,y] > .7f)
                {
                    riverStartList.Add(new Vector2(x, y));

                    Vector2 lowestNeighbour = GetLowestNeighbour(heightMap, riverPointsVect, x , y, width, height);

                    bool horizontal = lowestNeighbour.x != 0 ? false : true;

                    float lastVal = heightMap[x, y];
                    float minLastVal = lastVal;

                    int loop = 0;

                    while (loop < 100)
                    {
                        loop++;
                        if (horizontal)
                        {
                            lastVal = heightMap[sampleX, sampleY];
                            if (lastVal < minLastVal)
                            {
                                minLastVal = Mathf.Min(heightMap[sampleX, sampleY], heightMap[sampleX, sampleY+1]);
                            }

                        }
                        else
                        {
                            lastVal = heightMap[sampleX, sampleY];
                            if (lastVal < minLastVal)
                            {
                                minLastVal = Mathf.Min(heightMap[sampleX, sampleY], heightMap[sampleX+1, sampleY + 1]);
                            }
                        }
                        tempRiverPoints.Add(minLastVal);
                        riverPointsVect.Add(new Vector2(sampleX, sampleY));


                        lowestNeighbour = GetLowestNeighbour(heightMap, riverPointsVect, sampleX, sampleY, width, height);
                        horizontal = lowestNeighbour.x != 0 ? false : true;

                        sampleX += (int)lowestNeighbour.x;
                        sampleY += (int)lowestNeighbour.y;
  

                        if (!EvaluateRiverPointPosition(riverPointsVect, new Vector2(sampleX, sampleY)) || heightMap[sampleX, sampleY] <= .30f || (sampleX < 0 || sampleX > width) || (height < 0 || sampleY > height))
                        {
                            string th = " " + riverPointsVect.Count + " ";
                            th += " " + !EvaluateRiverPointPosition(riverPointsVect, new Vector2(sampleX, sampleY)) + " " +
                                 (heightMap[sampleX, sampleY] <= .30f) + " " + (sampleX < 0 || sampleX > width) + " " + (height < 0 || sampleY > height);
                            for (int i = 0; i < riverPointsVect.Count; i++)
                            {
                                th += riverPointsVect[i].x + "," + riverPointsVect[i].y + "  ";
                            }
                            Debug.Log(th);

                            break;
                        }
                    }


                    int xDir;
                    int yDir;

                    bool oldHorizontal = horizontal;
                    for (int i = 0; i < tempRiverPoints.Count; i++)
                    {
                        // Debug.Log(riverPointsVect[i].x + " " + riverPointsVect[i].y);

                        xDir = Mathf.Abs(x - (int)riverPointsVect[i].x);
                        yDir = Mathf.Abs(y - (int)riverPointsVect[i].y);

                        x = (int)riverPointsVect[i].x;
                        y = (int)riverPointsVect[i].y;

                        if (i == 0)
                        {
                            lastVal = tempRiverPoints[0];
                        }
                        riverPoints.Add(new Vector2(x, y));


                        if (i % 1 == 0)
                        {
                            lowestNeighbour = GetLowestNeighbour(heightMap, riverPointsVect, x, y, width, height);
                            oldHorizontal = horizontal;
                            horizontal = lowestNeighbour.x != 0 ? false : true;
                        }
                        

                        

                        //Debug.Log(xDir + " - " + yDir);

                        if (oldHorizontal != horizontal)
                        {
                            if (horizontal)
                            {
                                heightMap[x, y+1] = Mathf.Clamp(tempRiverPoints[i] - .085f, minLastVal + .0425f, lastVal - .085f);
                                heightMap[x+1, y + 1] = Mathf.Clamp(tempRiverPoints[i] - .085f, minLastVal + .0425f, lastVal - .085f);

                            }
                            else
                            {
                                heightMap[x + 2, y + 1] = Mathf.Clamp(tempRiverPoints[i] - .085f, minLastVal + .0425f, lastVal - .085f);
                                heightMap[x + 1, y + 1] = Mathf.Clamp(tempRiverPoints[i] - .085f, minLastVal + .0425f, lastVal - .085f);
                                heightMap[x + 1, y] = Mathf.Clamp(tempRiverPoints[i] - .085f, minLastVal + .0425f, lastVal - .085f);
                            }
                        }

                        heightMap[x, y] = Mathf.Clamp(tempRiverPoints[i] - .085f, minLastVal + .0425f, lastVal - .085f);

                        if (horizontal)
                        {
                            heightMap[x + 1, y] = Mathf.Clamp(tempRiverPoints[i] - .085f, minLastVal + .0425f, lastVal - .085f);
                        }
                        else
                        {
                            heightMap[x, y + 1] = Mathf.Clamp(tempRiverPoints[i] - .085f, minLastVal + .0425f, lastVal - .085f);
                        }

                        lastVal = tempRiverPoints[i];
                    }
                    riverPointsVect.Clear();
                    tempRiverPoints.Clear();
                }
            }
        }
        return new RiverReturn(riverPoints.ToArray(), heightMap);
    }

    public static bool EvaluateRiverStartPosition(List<Vector2> riverStartLocations, Vector2 location)
    {
        for (int i = 0; i < riverStartLocations.Count; i++)
        {
            if ( Mathf.Abs(riverStartLocations[i].x - location.x) < 12 && Mathf.Abs(riverStartLocations[i].y - location.y) < 12)
            {
                return false;
            }
        }

        return true;
    }

    public static bool EvaluateRiverPointPosition(List<Vector2> riverPoints, Vector2 location)
    {
        for (int i = 0; i < riverPoints.Count; i++)
        {
            if (riverPoints[i].x == location.x && riverPoints[i].y == location.y)
            {
                return false;
            }
        }

        return true;
    }

    private static Vector2 GetLowestNeighbour(float[,] heightMap, List<Vector2> riverPoints, int x, int y, int width, int height)
    {
        Dictionary<int, float> neighBoursHor = new Dictionary<int, float>();
        Dictionary<int, float> neighBoursVert = new Dictionary<int, float>();

        Vector2 origin = new Vector2(x, y);

        float xDir = 0;
        float yDir = 0;

        neighBoursHor.Clear();
        neighBoursVert.Clear();

        neighBoursHor.Add((x > 0) ? -1 : 0, (x > 0) ? heightMap[x - 1, y] : float.MaxValue);
        neighBoursHor.Add((x < width) ? 1 : 0, (x < width) ? heightMap[x + 1, y] : float.MaxValue);

        neighBoursVert.Add((x > 0) ? -1 : 0, (x > 0) ? heightMap[x, y - 1] : float.MaxValue);
        neighBoursVert.Add((y < height) ? 1 : 0, (y < height) ? heightMap[x, y + 1] : float.MaxValue);

        xDir = neighBoursHor.Aggregate((l, r) => l.Value > r.Value ? r : l).Value;
        yDir = neighBoursVert.Aggregate((l, r) => l.Value > r.Value ? r : l).Value;

        if (xDir < yDir)
        {
            xDir = neighBoursHor.Aggregate((l, r) => l.Value > r.Value ? r : l).Key;
            yDir = 0;
        }
        else
        {
            xDir = 0;
            yDir = neighBoursVert.Aggregate((l, r) => l.Value > r.Value ? r : l).Key;
        }

        Vector2 tempPos = new Vector2(origin.x + xDir, origin.y + yDir);

        return new Vector2(xDir, yDir);
    }

}

[System.Serializable]
public struct RiverReturn
{
    public Vector2[] riverPoints;
    public float[,] noiseMap;

    public RiverReturn(Vector2[] rp, float[,] nM)
    {
        riverPoints = rp;
        noiseMap = nM;
    }
}