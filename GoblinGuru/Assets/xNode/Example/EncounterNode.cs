using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[System.Serializable]
public class EncounterNode : Node
{
    [TextArea(10, 100)] public string text;

    public List<EncChoice> choices = new List<EncChoice>();
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

    public List<bool> triggerOnComplete;

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
        {
            Enc temp = new Enc(text, choices, terrainType, featureType);
            temp.requiredVariable = requiredVariable;
            temp.requiredVariableKey = requiredVariableKey;
            temp.rollSleep = rollSleep;
            temp.hasReward = hasReward;
            temp.hasCost = hasCost;

            temp.hpCost = hpCost;
            temp.staminaCost = staminaCost;
            temp.goldCost = goldCost;
            temp.itemCost = itemCost;

            temp.hpReward = hpReward;
            temp.staminaReward = staminaReward;
            temp.goldReward = goldReward;
            temp.itemReward = itemReward;

            temp.hasVariable = hasVariable;
            temp.variableKey = variableKey;
            temp.variableValue = variableValue;
            temp.opensEncounter = opensEncounter;
            temp.encounters = encounters;
            temp.triggerOnComplete = triggerOnComplete;
            return temp;
        }

        return false;
    }
}

