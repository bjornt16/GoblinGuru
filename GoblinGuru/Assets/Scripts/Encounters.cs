using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounters : MonoBehaviour {

    List<Encounter> encounters;

    public Encounters()
    {
        encounters = new List<Encounter>();
    }
    
    public void AddEncounter(Encounter enc)
    {
        encounters.Add(enc);
    }

    // Use this for initialization
    void Start()
    {
        Encounter e = new Encounter("Hungry grizzly bear");


        State s1 = new State("You notice a large grizzly bear strolling towards you", false);
        State s2 = new State("You are able to run away from the grizzly bear", true);
        State s3 = new State("The grizzly bear is able catches up to you and slashes you with its claws", true);
        e.AddState(s1);
        e.AddState(s2);
        e.AddState(s3);
        EncounterChoice c = new EncounterChoice("Try to flee from the grizzly", "dex", 0.6, s2, s3);
        e.states[0].AddChoice(c);

        e.PlayEncounter();
    }
}
