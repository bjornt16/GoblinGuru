using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[System.Serializable]
public class EncounterNode : Node
{
    [Output] public Enc encOutput;

    public TileTerrain terrainType = TileTerrain.Land;
    public TileFeatures featureType = TileFeatures.None;

    public bool requiredVariable;
    public string requiredVariableKey;
    public bool requiredVariableValue;

    public bool hasCost;
    public bool hasReward;

    public int hpCost;
    public int staminaCost;
    public int goldCost;
    public string itemCost;

    public CardObject itemReward;

    public int hpReward;
    public int staminaReward;
    public int goldReward;
    public SleepType rollSleep;

    public bool hasVariable;
    public string variableKey;
    public bool variableValue;

    public bool opensEncounter;
    public List<string> encounters;

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
            return new Enc(text, choices, terrainType, featureType);

        return false;
    }
}

