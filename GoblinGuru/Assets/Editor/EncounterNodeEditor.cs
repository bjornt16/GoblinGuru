using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(EncounterNode))]
public class EncounterNodeEditor : NodeEditor
{
    private SerializedObject serialObject;
    public override void OnBodyGUI()
    {
        EncounterNode encounter = target as EncounterNode;
        GUILayout.BeginHorizontal();
        //NodeEditorGUILayout.PortField(target.GetInputPort("input"), GUILayout.Width(100));
        EditorGUILayout.Space();
        NodeEditorGUILayout.PortField(new GUIContent("Output"), target.GetOutputPort("encOutput"), GUILayout.Width(50));

        GUILayout.EndHorizontal();
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

            new GUIContent("Win");
            encounter.choices[i].cText = EditorGUILayout.TextArea(encounter.choices[i].cText);

            if (!encounter.choices[i].MustHaveItemType)
            {
                encounter.choices[i].MustHaveItem = EditorGUILayout.Toggle(new GUIContent("Must Have Item"), encounter.choices[i].MustHaveItem);
                if (encounter.choices[i].MustHaveItem)
                {
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

        if (GUILayout.Button("Check Win", GUILayout.Width(30)))
        {
            Debug.Log(encounter.choices[0].win.text);
        }
    }
    public override int GetWidth()
    {
        return 400;
    }
}