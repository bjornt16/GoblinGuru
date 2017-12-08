using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

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
            encounter.choices[i].cText = EditorGUILayout.TextField(new GUIContent("Choices"), encounter.choices[i].cText);

            encounter.choices[i].rollType = EditorGUILayout.TextField(new GUIContent("Roll Type"), encounter.choices[i].rollType);

            encounter.choices[i].chance = EditorGUILayout.DoubleField(new GUIContent("Chance"), encounter.choices[i].chance);

            encounter.choices[i].win =  (Enc)target.GetInputPort(encounter.choices[i].winPort).GetInputValue();
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
            encounter.choices.Add(new EncChoice("", winport.fieldName, lossport.fieldName, "basic", 100.0, null, null));
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