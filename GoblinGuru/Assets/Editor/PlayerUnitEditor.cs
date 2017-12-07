using UnityEngine;
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
            playerUnit.GoUp();
        }
        if (GUILayout.Button("Move Down"))
        {
            playerUnit.GoDown();
        }
        if (GUILayout.Button("Move Left"))
        {
            playerUnit.GoLeft();
        }
        if (GUILayout.Button("Move Right"))
        {
             playerUnit.GoRight();  
        }
    }
}
