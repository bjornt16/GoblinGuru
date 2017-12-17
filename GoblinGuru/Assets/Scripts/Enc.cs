using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enc {

    public string name;
    public string text;

    public List<EncChoice> choices = new List<EncChoice>();

    public bool requiredVariable;
    public string requiredVariableKey;
    public bool requiredVariableValue;

    public TileTerrain terrainType;

    public TileFeatures featureType;

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

    public Enc(string t, List<EncChoice> c, TileTerrain tt, TileFeatures tf)
    {
        text = t;
        choices = c;
        terrainType = tt;
        featureType = tf;

        encounters = new List<string>();
    }


    public void PlayEncounter()
    {

        PlayTurn();
    }

    public Enc PlayTurn(int choice)
    {
        Debug.Log(choices[choice].cText);


        return choices[choice].AttemptChoice();

    }

    public void PlayTurn()
    {
        Debug.Log(text);
        for (int i = 0; i < choices.Count; i++)
        {
            Debug.Log(i + " - " + choices[i].cText);
        }
        int numChoice = 0;
        choices[numChoice].AttemptChoice();
    }
}
