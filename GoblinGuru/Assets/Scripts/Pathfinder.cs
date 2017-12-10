using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{

    static PathTilePriorityQueue searchFrontier;
    static int searchFrontierPhase;

    static PathTrail previewTrail = new PathTrail();
    static PathTrail tempTrail = new PathTrail();

    public static void PreviewPath(PlayerUnit player, GameTile fromTile, GameTile toTile)
    {
        Clear();
        tempTrail.Clear();
        tempTrail = GetPathTrail(player, (player.PathTrail != null ? player.PathTrail.PathTo.Tile.PathTile : fromTile.PathTile), toTile.PathTile);
        if (tempTrail != null)
        {
            if (player.PathTrail != null)
            {
                previewTrail = new PathTrail(player.PathTrail);
                previewTrail.extendPath(tempTrail);
            }
            else
            {
                previewTrail = tempTrail;
            }
            previewTrail.CalculateTurns(player);
            previewTrail.ShowPath();
        }
        else
        {
            tempTrail = new PathTrail();
        }
    }

    public static void AddPath(PlayerUnit player, GameTile fromTile, GameTile toTile)
    {
        player.PathTrail = new PathTrail(previewTrail);
    }

    private static PathTrail GetPathTrail(PlayerUnit player, PathTile fromTile, PathTile toTile)
    {
        searchFrontierPhase += 2;

        if (searchFrontier == null)
        {
            searchFrontier = new PathTilePriorityQueue();
        }
        else
        {
            searchFrontier.Clear();
        }

        fromTile.SearchPhase = searchFrontierPhase;
        fromTile.Distance = player.PathTrail == null ? 0 : player.PathTrail.PathTo.Distance;

        searchFrontier.Enqueue(fromTile);

        //references
        int distance = 0;
        int movePoints = player.MaxMovePoints;
        int oldPriority = 0;
        PathTile current = null;
        PathTile neighbor = null;
        
        //HexEdgeType edgeType;

        while (searchFrontier.Count > 0)
        {
            current = searchFrontier.Dequeue();
            current.SearchPhase += 1;

            for (int d = 0; d <= 3; d++)
            {
                neighbor = current.Tile.GetNeighbour(d) != null ? current.Tile.GetNeighbour(d).PathTile : null;
                if (neighbor == null || neighbor.Tile == null || neighbor.SearchPhase > searchFrontierPhase)
                {
                    continue;
                }

                /*
                if (neighbor.Tile.IsUnderwater || neighbor.Tile.Unit)
                {
                    continue;
                }
                edgeType = current.Tile.GetEdgeType(neighbor.Tile);
                if (edgeType == HexEdgeType.Cliff)
                {
                    continue;
                }
                */

                int moveCost = 1;

                /*
                
                if (neighbor.Tile.HasRiver)
                {
                    moveCost = current.Tile.HasRoadThroughEdge(d) ? 10 : 20;
                }
                else if (current.Tile.HasRoadThroughEdge(d))
                {
                    moveCost = 4;
                }
                else if (current.Tile.Walled != neighbor.Tile.Walled)
                {
                    continue;
                }
                else
                {
                    moveCost = edgeType == HexEdgeType.Flat ? 5 : 10;
                    moveCost += neighbor.Tile.UrbanLevel + neighbor.Tile.FarmLevel + neighbor.Tile.PlantLevel;
                }
                */

                distance = current.Distance + moveCost;

                if (neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.MoveCost = moveCost;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic = neighbor.Tile.DistanceTo(toTile.Tile.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    searchFrontier.Change(neighbor, oldPriority);
                }
            }

            if (current == toTile)
            {
                return new PathTrail(fromTile, toTile);
            }
        }
        return null;
    }

    public static void Clear()
    {
        tempTrail.Clear();
        previewTrail.Clear();
    }

}
