using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTrail
{

    public PathTile PathFrom;
    public PathTile PathTo;
    public List<PathTile> PathFromTo;
    public LinkedList<PathTile> PathBreak;
    private bool Visible = false;

    public PathTrail()
    {
        PathFromTo = new List<PathTile>();
        PathBreak = new LinkedList<PathTile>();
    }

    public PathTrail(PathTile from, PathTile to)
    {
        PathFromTo = new List<PathTile>();
        PathBreak = new LinkedList<PathTile>();
        PathTile newPath;
        for (PathTile c = to; c != from; c = c.PathFrom)
        {
            newPath = new PathTile(c);
            PathFromTo.Add(newPath);
        }
        PathFromTo.Add(new PathTile(from));
        PathTo = PathFromTo[0];
        PathFrom = PathFromTo[PathFromTo.Count - 1];

        PathFromTo.Reverse();
    }

    public PathTrail(PathTrail copyFrom)
    {
        PathFromTo = new List<PathTile>(copyFrom.PathFromTo);
        PathBreak = new LinkedList<PathTile>(copyFrom.PathBreak);
        if (PathFromTo.Count != 0)
        {
            PathFrom = copyFrom.PathFromTo[0];
            PathTo = copyFrom.PathFromTo[copyFrom.PathFromTo.Count - 1];
        }
    }

    public bool ContainsTile(GameTile findTile)
    {
        for (int i = 0; i < PathFromTo.Count; i++)
        {
            if(PathFromTo[i].Tile == findTile)
            {
                return true;
            }
        }
        return false;
    }

    public PathTile getNextPathBreak()
    {
        return PathBreak.First != null ? PathBreak.First.Value : PathTo;
    }

    public int getBreakIndex(PathTile pathTile)
    {
        return PathFromTo.IndexOf(pathTile);
    }

    public void Clear()
    {
        HidePath();
        PathFromTo.Clear();
        PathBreak.Clear();
        PathFrom = null;
        PathTo = null;
    }

    public void ShowPath()
    {

        int length = PathFromTo.Count;
        for (int i = 1; i < length; i++)
        {
            PathFromTo[i].Tile.Highlight(true, Color.white, PathFromTo[i].Turn.ToString());
        }
        PathTo.Tile.Highlight(true, Color.red, PathTo.Turn.ToString());
        Visible = true;

    }

    public void HidePath()
    {
        if (Visible)
        {
            int length = PathFromTo.Count;
            for (int i = 1; i < length; i++)
            {
                PathFromTo[i].Tile.Highlight(false);
            }
            Visible = false;
        }

    }

    public void ClearUsedTiles(PlayerUnit player)
    {
        int length = this.PathFromTo.Count;
        int removeCount = 0;

        if (PathFrom.Tile != player.Tile)
        {
            for (int i = 0; i < length; i++)
            {

                if (PathFromTo[i].Tile == player.Tile)
                {
                    removeCount = i;
                    break;
                }
                else
                {
                    PathFromTo[i].Tile.Highlight(false);
                }
            }

            if (length - 1 == removeCount)
            {
                Clear();
            }
            else
            {
                PathFromTo.RemoveRange(0, removeCount);
                PathFrom = PathFromTo[0];
            }

        }
    }

    public void CalculateTurns(PlayerUnit player)
    {
        int movesPerTurn = player.MaxMovePoints;
        int currentMoves = player.CurrentMovePoints;

        PathBreak.Clear();

        int lastTurn = 0;
        int currentTurn = 0;
        int distance = movesPerTurn - currentMoves;
        int length = this.PathFromTo.Count;
        for (int i = 1; i < length; i++)
        {
            PathTile current = PathFromTo[i];
            distance += current.MoveCost;
            current.Distance = distance;
            currentTurn = (distance - 1) / movesPerTurn;
            current.Turn = currentTurn;
            current.isBreak = false;

            if (currentTurn > lastTurn)
            {
                distance = (movesPerTurn * currentTurn) + current.MoveCost;
                current.Distance = distance;
                PathFromTo[i - 1].isBreak = true;
                PathBreak.AddLast(PathFromTo[i - 1]);
            }
            lastTurn = currentTurn;
        }
    }

    public void extendPath(PathTrail extP)
    {
        if (this.PathTo.Tile == extP.PathFrom.Tile && extP.PathFromTo.Count != 0) //check if the paths line up
        {
            extP.PathFrom.PathFrom = this.PathTo.PathFrom;
            int length = extP.PathFromTo.Count;
            for (int i = 1; i < length; i++)
            {
                this.PathFromTo.Add(extP.PathFromTo[i]);
            }
            this.PathTo = extP.PathTo;
        }
        else
        {
            Debug.Log("Path to Path Extension connections don't match.");
            Debug.Log(this.PathTo.Tile + " " + extP.PathFrom.Tile);
        }
    }

}
