﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour {

    Vector3 position;
    public Vector2 coordinates;

    public GameTile tileUp;
    public GameTile tileDown;
    public GameTile tileLeft;
    public GameTile tileRight;

    public int tileVerticeFrom;
    public int tileVerticeTo;

    public TileTerrain tileTerrain;

    public GameTileLabel labelPrefab;
    public GameTileLabel label;

    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }

    public void Initialize(Transform parent, int x, int y, TileTerrain tT)
    {
        name = "x: " + x + ", y:" + y;
        coordinates = new Vector2(x, y);
        tileTerrain = tT;
        SetPositionRaycast(transform.position, Vector3.down);
        if(parent != null)
        {
            transform.parent = parent;
        }

        label.Initialize(x, y);
    }

    public void SetNeighbours(GameTile up, GameTile down, GameTile left, GameTile right)
    {
        //tileUp = up;
        tileDown = down;
        tileLeft = left;
        //tileRight = right;

        if(down != null)
        {
            down.tileUp = this;
        }
        if (left != null)
        {
            left.tileRight = this;
        }
    }

    private void SetPositionRaycast(Vector3 positon, Vector3 direction)
    {
        Ray ray = new Ray(positon, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
            position = transform.position;
        }
    }

    public void Highlight(bool highlight)
    {
        label.gameObject.SetActive(highlight);
    }

}
