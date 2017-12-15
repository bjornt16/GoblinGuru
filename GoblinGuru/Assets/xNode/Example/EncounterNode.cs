using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[System.Serializable]
public class EncounterNode : Node
{
    [Output] public Enc encOutput;

    [TextArea (10,100)] public string text;
    public List<EncChoice> choices = new List<EncChoice>();

    // GetValue should be overridden to return a value for any specified output port
    public override object GetValue(NodePort port)
    {
        // After you've gotten your input values, you can perform your calculations and return a value
        if (port.fieldName == "text")
            return text;
        if (port.fieldName == "test")
            if (GetInputPort("test") != null)
                return GetInputPort("test").GetInputValue();
            else
                return text;
        if (port.fieldName == "encOutput")
            return new Enc(text, choices);

        return false;
    }
}

