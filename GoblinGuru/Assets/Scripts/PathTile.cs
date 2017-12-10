using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile
{
    public PathTile(GameTile tile)
    {
        Tile = tile;
        Tile.PathTile = this;
        OnMap = true;
    }

    public PathTile(PathTile copyFrom)
    {
        Tile = copyFrom.Tile;
        this.PathFrom = copyFrom.PathFrom;
        this.distance = copyFrom.Distance;
        this.Turn = copyFrom.Turn;
        this.isBreak = copyFrom.isBreak;
        this.moveCost = copyFrom.MoveCost;
    }

    public bool OnMap = false;
    public bool isBreak = false;

    public GameTile Tile { get; set; }

    public PathTile PathFrom { get; set; }

    public int SearchHeuristic { get; set; }

    public PathTile NextWithSamePriority { get; set; }

    public int SearchPhase { get; set; }

    public int SearchPriority
    {
        get
        {
            return distance + SearchHeuristic;
        }
    }

    private int distance;

    private int moveCost;

    public int MoveCost
    {
        get { return moveCost; }
        set
        {
            moveCost = value;
        }
    }

    public int Distance
    {
        get { return distance; }
        set
        {
            distance = value;
        }
    }

    public int Turn { get; set; }
}
