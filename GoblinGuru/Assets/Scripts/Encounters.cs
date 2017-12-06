using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounters : MonoBehaviour {

    public EncounterUI ui;

    List<Encounter> encounters;
    List<OptionButton> buttons;

    public Encounters()
    {
        encounters = new List<Encounter>();
    }
    
    public void AddEncounter(Encounter enc)
    {
        encounters.Add(enc);
    }

    public void Initialize()
    {
        buttons = new List<OptionButton>();
        ui.stateText.text = encounters[0].currentState.text;

        if(encounters[0].currentState.choices.Count == 0)
        {
            UnityEngine.UI.Button tempButton = Instantiate(ui.buttonPrefab, ui.options.transform).GetComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Text tempText = tempButton.gameObject.transform.GetComponentInChildren<UnityEngine.UI.Text>();
            tempText.text = "Continue";
            tempButton.onClick.AddListener(() => CloseDialogue());
        }
        
        for (int i = 0; i < encounters[0].currentState.choices.Count; i++)
        {
            OptionButton tempButton = Instantiate(ui.buttonPrefab, ui.options.transform);
            tempButton.Initialize(i, encounters[0].currentState.choices[i].cText);
            buttons.Add(tempButton);
            tempButton.option.onClick.AddListener(() => OnClickTask(tempButton.parameter));
            /*UnityEngine.UI.Text tempText = tempButton.gameObject.transform.GetComponentInChildren<UnityEngine.UI.Text>();
            tempText.text = encounters[0].currentState.choices[i].cText;
            Debug.Log("i is: " + i);
            tempButton.onClick.AddListener(() => OnClickTask(counter));
            */
        }

    }

    public void OnClickTask(int number)
    {
        Debug.Log(number);
        encounters[0].PlayTurn(number);
        foreach(OptionButton button in buttons)
        {
            button.Destroy();
        }
        Initialize();
    }

    public void CloseDialogue()
    {
        //todo functionality
    }

    // Use this for initialization
    void Start()
    {
        Encounter e = new Encounter("Hungry grizzly bear");


        State s0 = new State("You notice a large grizzly bear strolling towards you", false);
        State s1 = new State("You are able to run away from the grizzly bear", true);
        State s2 = new State("The grizzly bear is able catches up to you and mauls you with its paw", false);
        State s3 = new State("You attack the grizzly with great ferocity visibly hurting the animal", true);
        State s4 = new State("You attempt to strike the grizzly but the beast bites your hand as you are about to land the blow", false);
        e.AddState(s0);
        e.AddState(s1);
        e.AddState(s2);
        e.AddState(s3);
        e.AddState(s4);
        //e.AddState(s5);
        EncounterChoice c1 = new EncounterChoice("Try to flee from the grizzly", "dex", 0.0, e.states[1], e.states[2]);
        EncounterChoice c2 = new EncounterChoice("Stand your ground and attempt to attack the grizzly", "str", 1.0, e.states[3], e.states[4]);
        e.states[0].AddChoice(c1);
        e.states[0].AddChoice(c2);
        e.states[2].AddChoice(c2);
        AddEncounter(e);


        Initialize();
        //e.PlayEncounter();
    }
}
