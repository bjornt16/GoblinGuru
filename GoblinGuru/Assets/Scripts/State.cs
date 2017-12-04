using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {

    public List<EncounterChoice> choices;
    public string text;
    public bool endState = false;

    public State(string newText, bool end)
    {
        choices = new List<EncounterChoice>();
        text = newText;
        endState = end;
    }

    public void AddChoice(EncounterChoice c)
    {
        choices.Add(c);
    }
}
