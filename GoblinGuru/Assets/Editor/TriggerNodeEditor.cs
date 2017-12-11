using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;


[CustomNodeEditor(typeof(TriggerNode))]
public class TriggerNodeEditor : NodeEditor
{
    public override int GetWidth()
    {
        return 336;
    }
}
