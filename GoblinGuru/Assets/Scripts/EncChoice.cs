using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[System.Serializable]
public class EncChoice
{
    public string winPort;
    public string lossPort;
    [TextArea] public string cText;
    public string rollType;
    public double chance;
    [System.NonSerialized]
    public Enc win;
    [System.NonSerialized]
    public Enc loss;

    public EncChoice(string newCText, string newWinPort, string newLossPort, string newRollType, double newChance, Enc newWin, Enc newLoose)
    {
        winPort = newWinPort;
        lossPort = newLossPort;
        cText = newCText;
        rollType = newRollType;
        chance = newChance;
        win = newWin;
        loss = newLoose;
    }

    public Enc AttemptChoice()
    {
        double randomNumber = Random.Range(0.0f, 1.0f);
        Debug.Log(randomNumber);
        if (randomNumber < chance)
        {
            return win;
        }
        return loss;
    }
}
