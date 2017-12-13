using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Combat))]
public class CombatEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Combat combat = (Combat)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Set Melee Range"))
        {
            combat.CombatUI.SetRangeMelee(combat.playerModel, combat.targetModel);
        }
        if (GUILayout.Button("Set Ranged Range"))
        {
            combat.CombatUI.SetRangeRanged(combat.playerModel, combat.targetModel);
        }
        if (GUILayout.Button("Set Distant Range"))
        {
            combat.CombatUI.SetRangeDistant(combat.playerModel, combat.targetModel);
        }
    }
}
