using System.Collections.Generic;

public class PathTilePriorityQueue
{

    List<PathTile> list = new List<PathTile>();

    int count = 0;

    public int Count
    {
        get
        {
            return count;
        }
    }

    int minimum = int.MaxValue;

    public void Enqueue(PathTile tile)
    {
        count += 1;
        int priority = tile.SearchPriority;
        if (priority < minimum)
        {
            minimum = priority;
        }
        while (priority >= list.Count)
        {
            list.Add(null);
        }
        tile.NextWithSamePriority = list[priority];
        list[priority] = tile;
    }

    public PathTile Dequeue()
    {
        count -= 1;
        for (; minimum < list.Count; minimum++)
        {
            PathTile tile = list[minimum];
            if (tile != null)
            {
                list[minimum] = tile.NextWithSamePriority;
                return tile;
            }
        }
        return null;
    }

    public void Change(PathTile tile, int oldPriority)
    {
        PathTile current = list[oldPriority];
        PathTile next = current.NextWithSamePriority;
        if (current == tile)
        {
            list[oldPriority] = next;
        }
        else
        {
            while (next != tile)
            {
                current = next;
                next = current.NextWithSamePriority;
            }
            current.NextWithSamePriority = tile.NextWithSamePriority;
        }
        Enqueue(tile);
        count -= 1;
    }

    public void Clear()
    {
        count = 0;
        list.Clear();
        minimum = int.MaxValue;
    }
}