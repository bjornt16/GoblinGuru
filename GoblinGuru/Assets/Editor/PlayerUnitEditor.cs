﻿using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(PlayerUnit))]
public class PlayerUnitEditor : Editor
{

    public override void OnInspectorGUI()
    {
        PlayerUnit playerUnit = (PlayerUnit)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Move to destination"))
        {
            playerUnit.Move();
        }

        if (GUILayout.Button("Move Up"))
        {
            if(playerUnit.Tile.tileUp != null)
            {
                playerUnit.goUp();
            }

        }
        if (GUILayout.Button("Move Down"))
        {
            if (playerUnit.Tile.tileDown != null)
            {
                playerUnit.goDown();
            }
        }
        if (GUILayout.Button("Move Left"))
        {
            if (playerUnit.Tile.tileLeft != null)
            {
                playerUnit.goLeft();
            }
        }
        if (GUILayout.Button("Move Right"))
        {
            if (playerUnit.Tile.tileRight != null)
            {
                playerUnit.goRight();
            }
        }
    }
}
