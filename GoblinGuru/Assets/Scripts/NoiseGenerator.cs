using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator {

	public static float[,] GenerateNoise(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset){
        float[,] noise = new float[width, height];

        System.Random rng = new System.Random(seed);
        Vector2[] octaveOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++){
            float offsetX = rng.Next(-100000, 100000) + offset.x;
            float offsetY = rng.Next(-100000, 100000) + offset.y;
            octaveOffset[i] = new Vector2(offsetX, offsetY);
        }

        if(scale <= 0){
            scale = 0.0001f;
        }

        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;

        float halfWidth = width / 2;
        float halfHeight = height / 2;

        int x;
        for (int y = 0; y < height; y++){
            for (x = 0; x < width; x++){

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int k = 0; k < octaves; k++){

                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffset[k].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffset[k].y;

                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += noiseValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }


                noise[x, y] = Mathf.Round(noiseHeight*10) / 10;
            }

        }

        /*
        float prevX;
        float prevY;
        //normalize
        for ( x = 2; x < width - 2; x += 2)
        {
            for (int y = 2; y < height - 2; y += 2)
            {
                prevX = (noise[x - 2, y] + noise[x - 1, y]) / 2;
                prevY = (noise[x, y - 2] + noise[x, y - 1]) / 2;
                noise[x, y] = Mathf.Min(Mathf.Clamp(noise[x, y], prevY - .1f, prevY + .1f) , Mathf.Clamp(noise[x, y], prevX-.1f, prevX + .1f) ); 
                noise[x + 1, y] = noise[x, y];
                noise[x, y + 1] = noise[x, y];

                noise[x + 1, y + 1] = noise[x, y];
            }
        }
        */
        

        for (x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                noise[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noise[x, y]);
            }
        }

        return noise;
    }
}
