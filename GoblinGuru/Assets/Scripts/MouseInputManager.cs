using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInputManager : MonoBehaviour
{

    private static MouseInputManager instance = null;
    public static MouseInputManager Instance { get { return instance; } }

    public Map map;

    public Vector2 temp;
    public Vector2 current;
    public GameTile currentTile;
    public GameTile selectedTile;

    public PlayerUnit SelectedUnit;

    bool isPathfinding;

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

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))  //if left clicking.
            {
                DoSelection();
            }
            else if (Input.GetMouseButtonDown(1) && SelectedUnit != null && !SelectedUnit.Moving)
            {
                DoAction();
            }
            else if (SelectedUnit != null && isPathfinding)
            {
                PreviewPath();
            }
        }
    }

    bool UpdateCurrentTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50f))
        {
            //draw invisible ray cast/vector
            //Debug.DrawLine(ray.origin, hit.point);
            //log hit area to the console

            temp = RoundCoordinates(hit.point);
            GameTile tile = map.GetTile((int)temp.x, (int)temp.y);
            if (tile == null)
            {
                return false;
            }
            else if (tile != currentTile)
            {
                currentTile = tile;
                return true;
            }
        }
        return false;
    }


    void DoSelection() //left click
    {
        UpdateCurrentTile();
        if (currentTile == null) //didnt click on a tile, return
        {
            return;
        }
        else if (isPathfinding) //if pathfinding is on, turn off pathfinding and hide pathfinding preview. (and reshow unit path)
        {
            isPathfinding = false;
            Pathfinder.Clear();
            if (SelectedUnit.PathTrail != null)
            {
                SelectedUnit.PathTrail.ShowPath();
            }
        }
        else if (!isPathfinding && !SelectedUnit.Moving && SelectedUnit.PathTrail != null && !SelectedUnit.PathTrail.ContainsTile(currentTile)) //else unselecting unit.
        {

            SelectedUnit.PathTrail.HidePath();
            SelectedUnit.PathTrail = null;
            
            Pathfinder.Clear();

        }
    }

    void DoAction() //rightClick
    {
        UpdateCurrentTile();
        if (currentTile == null || !SelectedUnit.ValidateMove(currentTile)) //didnt click on a hex tile, return
        {
            return;
        }
    
        if (SelectedUnit.PathTrail != null && currentTile == SelectedUnit.PathTrail.PathTo.Tile) //right click on path end, do move action.
        {
            DoMove();
            isPathfinding = false;
        }
        else if (isPathfinding) //if pathfinding, add the path.
        {
            AddPath();
        }
        else //turn on pathfinding, force show path (force is required to show the path instantly, else path would not be shown until cursor moves out of current tile)
        {
            isPathfinding = true;
            PreviewPath(true);
        }
    }

    void AddPath()
    {
        Pathfinder.AddPath(SelectedUnit, SelectedUnit.Tile, currentTile);
    }

    void PreviewPath(bool forcePreview = false)
    {
        if ((UpdateCurrentTile() || forcePreview))// && SelectedUnit.IsValidDestination(currentCell))
        {
            if (SelectedUnit.ValidateMove(currentTile))
            {
                Pathfinder.PreviewPath(SelectedUnit, SelectedUnit.Tile, currentTile);
            }
            
        }
    }

    void DoMove()
    {
        SelectedUnit.Move();
    }


    private Vector2 RoundCoordinates(Vector3 coords)
    {
        float roundX = Mathf.Floor(coords.x);
        float roundY = Mathf.Floor(coords.z);

        return new Vector2(roundX, roundY);
    }

}
