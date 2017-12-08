using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterChoice {
    public string portName;
    public string cText;
    public string rollType;
    public double chance;
    State win;
    State loose;

    public EncounterChoice(string newCText, string newPortName, string newRollType, double newChance, State newWin, State newLoose)
    {
        portName = newPortName;
        cText = newCText;
        rollType = newRollType;
        chance = newChance;
        win = newWin;
        loose = newLoose;
    }

    public State AttemptChoice()
    {
        double randomNumber = Random.Range(0.0f, 1.0f);
        Debug.Log(randomNumber);
        if (randomNumber < chance)
        {
            return win;
        }
        return loose;
    }
}
