using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounters : MonoBehaviour {

    public EncounterUI ui;

    List<Encounter> randomEncounters;
    List<OptionButton> buttons;

    Encounter curr;
    State currOriginalState;

    public Encounters()
    {
        randomEncounters = new List<Encounter>();
    }
    
    public void AddEncounter(Encounter enc)
    {
        randomEncounters.Add(enc);
    }

    public void pickRandom()
    {
        int randomNum = 0;
        curr = randomEncounters[0];
        currOriginalState = curr.currentState;
        Initialize();
    }

    public void Initialize()
    {
        buttons = new List<OptionButton>();
        ui.stateText.text = curr.currentState.text;

        if(curr.currentState.choices.Count == 0)
        {
            OptionButton tempButton = Instantiate(ui.buttonPrefab, ui.options.transform);
            tempButton.Initialize(0, "Continue");
            //UnityEngine.UI.Button tempButton = Instantiate(ui.buttonPrefab, ui.options.transform).GetComponent<UnityEngine.UI.Button>();
            //UnityEngine.UI.Text tempText = tempButton.gameObject.transform.GetComponentInChildren<UnityEngine.UI.Text>();
            buttons.Add(tempButton);
            tempButton.option.onClick.AddListener(() => CloseDialogue());
        }
        
        for (int i = 0; i < curr.currentState.choices.Count; i++)
        {
            OptionButton tempButton = Instantiate(ui.buttonPrefab, ui.options.transform);
            tempButton.Initialize(i, curr.currentState.choices[i].cText);
            buttons.Add(tempButton);
            tempButton.option.onClick.AddListener(() => OnClickTask(tempButton.parameter));
            /*UnityEngine.UI.Text tempText = tempButton.gameObject.transform.GetComponentInChildren<UnityEngine.UI.Text>();
            tempText.text = encounters[0].currentState.choices[i].cText;
            Debug.Log("i is: " + i);
            tempButton.onClick.AddListener(() => OnClickTask(counter));
            */
        }
        ui.gameObject.SetActive(true);
    }

    public void OnClickTask(int number)
    {
        Debug.Log(number);
        FindObjectOfType<AudioManager>().Play("buttonClick");
        curr.PlayTurn(number);
        foreach(OptionButton button in buttons)
        {
            button.Destroy();
        }
        Initialize();
    }

    public void CloseDialogue()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");
        //todo functionality
        ui.gameObject.SetActive(false);
        foreach (OptionButton button in buttons)
        {
            button.Destroy();
        }
        curr.currentState = currOriginalState;
        GameStateManager.Instance.startMovement();
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


        //Initialize();
        //e.PlayEncounter();
    }
}
