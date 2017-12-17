using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;
using System.Collections;
using System.Collections.Generic;

[CustomNodeEditor(typeof(EncounterNode))]
public class EncounterNodeEditor : NodeEditor
{
    
    public override void OnBodyGUI()
    {
        EncounterNode encounter = target as EncounterNode;
        GUILayout.BeginHorizontal();
        //NodeEditorGUILayout.PortField(target.GetInputPort("input"), GUILayout.Width(100));
        EditorGUILayout.Space();
        NodeEditorGUILayout.PortField(new GUIContent("Output"), target.GetOutputPort("encOutput"), GUILayout.Width(50));

        GUILayout.EndHorizontal();

        if (target == target.graph.nodes[0])
        {
            GUIStyle style = new GUIStyle();
            style.richText = true;
            GUILayout.Label("<size=20><color=yellow>Encounter <b>Start</b></color></size>", style);
            encounter.requiredVariable = EditorGUILayout.Toggle(new GUIContent("Must Have Variable"), encounter.requiredVariable);
            if (encounter.requiredVariable)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("requiredVariableKey"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("requiredVariableValue"));
            }
            encounter.terrainType = (TileTerrain)EditorGUILayout.EnumPopup(new GUIContent("Terrain Type"), encounter.terrainType);
            encounter.featureType = (TileFeatures)EditorGUILayout.EnumPopup(new GUIContent("Terrain Type"), encounter.featureType);
        }
        else
        {
            GUIStyle style = new GUIStyle();
            style.richText = true;
            GUILayout.Label("<size=15><color=yellow>Encounter <b>Branch</b></color></size>", style);
            encounter.hasCost = EditorGUILayout.Toggle(new GUIContent("Has Cost"), encounter.hasCost);
            if (encounter.hasCost)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("hpCost"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("staminaCost"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("goldCost"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("itemCost"));
            }
            encounter.hasReward = EditorGUILayout.Toggle(new GUIContent("Has Reward"), encounter.hasReward);
            if (encounter.hasReward)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("hpReward"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("staminaReward"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("goldReward"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("itemReward"));
                encounter.rollSleep = (SleepType)EditorGUILayout.EnumPopup(new GUIContent("Sleep Type"), encounter.rollSleep);
            }
            encounter.hasVariable = EditorGUILayout.Toggle(new GUIContent("Set Variable"), encounter.hasVariable);
            if (encounter.hasVariable)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("variableKey"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("variableValue"));
            }
        }
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("text"));



        for (int i = 0; i < encounter.choices.Count; i++)
        {
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                target.RemoveInstancePort(encounter.choices[i].winPort);
                target.RemoveInstancePort(encounter.choices[i].lossPort);
                encounter.choices.RemoveAt(i);
                i--;
            }
            GUIStyle style = new GUIStyle();
            style.richText = true;
            GUILayout.Label("<size=10><color=yellow>Encounter <b>Choice</b></color></size>", style);

            EditorStyles.textField.wordWrap = true;
            encounter.choices[i].cText = EditorGUILayout.TextArea(encounter.choices[i].cText, GUILayout.ExpandHeight(true), GUILayout.Height(30) );


            encounter.choices[i].mustHaveVariable = EditorGUILayout.Toggle(new GUIContent("Must Have Variable"), encounter.choices[i].mustHaveVariable);
            if (encounter.choices[i].mustHaveVariable)
            {
                encounter.choices[i].variableKey = EditorGUILayout.TextField(new GUIContent("Key"), encounter.choices[i].variableKey);
                encounter.choices[i].variableValue = EditorGUILayout.Toggle(new GUIContent("Value"), encounter.choices[i].variableValue);
            }

            if (!encounter.choices[i].MustHaveItemType)
            {
                encounter.choices[i].MustHaveItem = EditorGUILayout.Toggle(new GUIContent("Must Have Item"), encounter.choices[i].MustHaveItem);
                if (encounter.choices[i].MustHaveItem)
                {
                    encounter.choices[i].MustNotHaveItem = EditorGUILayout.Toggle(new GUIContent("Must Not Have Item"), encounter.choices[i].MustNotHaveItem);
                    encounter.choices[i].itemName = EditorGUILayout.TextField(new GUIContent("Item Name"), encounter.choices[i].itemName);
                }

            }

            if (!encounter.choices[i].MustHaveItem)
            {
                encounter.choices[i].MustHaveItemType = EditorGUILayout.Toggle(new GUIContent("Must Have Item Type"), encounter.choices[i].MustHaveItemType);
                if (encounter.choices[i].MustHaveItemType)
                {
                    encounter.choices[i].itemType = (CardType)EditorGUILayout.EnumPopup(new GUIContent("Item Type"), encounter.choices[i].itemType);
                }
            }


            encounter.choices[i].type = (EncChoiceType)EditorGUILayout.EnumPopup(new GUIContent("Choice Type"), encounter.choices[i].type);

            if (encounter.choices[i].type == EncChoiceType.roll)
            {
                encounter.choices[i].rollType = (CharacterStats)EditorGUILayout.EnumPopup(new GUIContent("Roll Type"), encounter.choices[i].rollType);
                encounter.choices[i].dc = EditorGUILayout.IntField(new GUIContent("DC"), encounter.choices[i].dc);
            }
            else if (encounter.choices[i].type == EncChoiceType.combat)
            {
                encounter.choices[i].combatTarget = (CombatEnemy)EditorGUILayout.ObjectField("Combat Target", encounter.choices[i].combatTarget, typeof(CombatEnemy), true);
            }
            else if (encounter.choices[i].type == EncChoiceType.statCheck)
            {
                encounter.choices[i].stat = (CharacterStatsCheck)EditorGUILayout.EnumPopup(new GUIContent("Stat Type"), encounter.choices[i].stat);
                encounter.choices[i].statAmount = EditorGUILayout.IntField(new GUIContent("Amount"), encounter.choices[i].statAmount);
            }

            encounter.choices[i].win = (Enc)target.GetInputPort(encounter.choices[i].winPort).GetInputValue();
            encounter.choices[i].loss = (Enc)target.GetInputPort(encounter.choices[i].lossPort).GetInputValue();

            NodeEditorGUILayout.PortField(new GUIContent("Win"), target.GetInputPort(encounter.choices[i].winPort), GUILayout.Width(350));
            NodeEditorGUILayout.PortField(new GUIContent("Loss"), target.GetInputPort(encounter.choices[i].lossPort), GUILayout.Width(350));

        }
        GUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("+", GUILayout.Width(30)))
        {
            NodePort winport = target.AddInstanceInput(typeof(Enc));
            NodePort lossport = target.AddInstanceInput(typeof(Enc));
            Debug.Log(winport.fieldName);
            encounter.choices.Add(new EncChoice("", winport.fieldName, lossport.fieldName, CharacterStats.D20, 10));
        }
        GUILayout.EndHorizontal();

        encounter.opensEncounter = EditorGUILayout.Toggle(new GUIContent("Opens other Encounter"), encounter.opensEncounter);
        if (encounter.opensEncounter)
        {
            if(encounter.encounters == null)
            {
                encounter.encounters = new List<string>();
            }
            for (int l = 0; l < encounter.encounters.Count; l++)
            {
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    encounter.encounters.RemoveAt(l);
                    l--;
                }
                encounter.encounters[l] = EditorGUILayout.TextField(new GUIContent("Encounter Name"), encounter.encounters[l]);

            }
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                encounter.encounters.Add("");
            }
        }


    }
    public override int GetWidth()
    {
        return 400;
    }
}