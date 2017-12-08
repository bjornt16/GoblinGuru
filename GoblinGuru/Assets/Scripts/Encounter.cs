using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Encounter {
    
    public string name;
    public List<State> states;
    public State currentState;
    public int HP;
    public int maxHP;
    

    public Encounter(string newName)
    {
        states = new List<State>();
        currentState = null;
        name = newName;
        HP = int.MaxValue;
        maxHP = int.MaxValue;
    }
    
    public Encounter(string newName, int newHp)
    {
        states = new List<State>();
        currentState = null;
        name = newName;
        HP = newHp;
        maxHP = newHp;
    }

    /*public State CurrentState()
    {
        return currentState;
    }*/

    public string CurrentText()
    {
        return currentState.text;
    }

    public List<EncounterChoice> AvailableChoices()
    {
        return currentState.choices;
    }
    
    public void PrintStates()
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i] == currentState)
            {
                Debug.Log("*");
            }
            Debug.Log("[" + i + "] - " + states[i].text);
        }
    }

    public void AddState(State s)
    {
        states.Add(s);
        if (states.Count == 1)
        {
            currentState = states[0];
        }
    }

    public void PlayEncounter()
    {
        while (!currentState.endState)
        {
            PlayTurn();
        }
        Debug.Log(currentState.text);
    }

    public void PlayTurn(int choice)
    {
        Debug.Log(currentState.choices[choice].cText);

        currentState = currentState.choices[choice].AttemptChoice();
    }

    public void PlayTurn()
    {
        Debug.Log(currentState.text);
        for (int i = 0; i < currentState.choices.Count; i++)
        {
            Debug.Log(i + " - " + currentState.choices[i].cText);
        }
        int numChoice = 0;
        currentState = currentState.choices[numChoice].AttemptChoice();
    }
}
