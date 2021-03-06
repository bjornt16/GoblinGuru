﻿using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI(){
        MapGenerator mapGenerator = (MapGenerator)target;

        if (DrawDefaultInspector()){
            if (mapGenerator.autoUpdate){
                mapGenerator.GenerateMap();
            }
        }

        if(GUILayout.Button("Generate Map")){
            mapGenerator.GenerateMap();
        }
    }
}
