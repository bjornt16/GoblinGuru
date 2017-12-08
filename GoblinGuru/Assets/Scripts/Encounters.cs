using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class Encounters : MonoBehaviour {

    public EncounterUI ui;

<<<<<<< HEAD
    List<Enc> encList;
    List<Encounter> encounters;
    List<OptionButton> buttons;

    int encIndex = 0;

    Enc currentEnc;
=======
    List<Encounter> randomEncounters;
    List<OptionButton> buttons;

    Encounter curr;
    State currOriginalState;
>>>>>>> f51b8ab26174d9664d6dc79a9051cb0921ab2376

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
<<<<<<< HEAD
        ui.stateText.text = currentEnc.text;

        if (currentEnc.choices.Count == 0)
=======
        ui.stateText.text = curr.currentState.text;

        if(curr.currentState.choices.Count == 0)
>>>>>>> f51b8ab26174d9664d6dc79a9051cb0921ab2376
        {
            OptionButton tempButton = Instantiate(ui.buttonPrefab, ui.options.transform);
            tempButton.Initialize(0, "Continue");
            //UnityEngine.UI.Button tempButton = Instantiate(ui.buttonPrefab, ui.options.transform).GetComponent<UnityEngine.UI.Button>();
            //UnityEngine.UI.Text tempText = tempButton.gameObject.transform.GetComponentInChildren<UnityEngine.UI.Text>();
            buttons.Add(tempButton);
            tempButton.option.onClick.AddListener(() => CloseDialogue());
        }
        else
        {
            for (int i = 0; i < currentEnc.choices.Count; i++)
            {
                Debug.Log("i " + i);
                OptionButton tempButton = Instantiate(ui.buttonPrefab, ui.options.transform);
                tempButton.Initialize(i, currentEnc.choices[i].cText);
                buttons.Add(tempButton);
                tempButton.option.onClick.AddListener(() => OnClickTask(tempButton.parameter));
                /*UnityEngine.UI.Text tempText = tempButton.gameObject.transform.GetComponentInChildren<UnityEngine.UI.Text>();
                tempText.text = encounters[0].currentState.choices[i].cText;
                Debug.Log("i is: " + i);
                tempButton.onClick.AddListener(() => OnClickTask(counter));*/
            }
        }


        /*

        
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
            
        }
<<<<<<< HEAD
        */

=======
        ui.gameObject.SetActive(true);
>>>>>>> f51b8ab26174d9664d6dc79a9051cb0921ab2376
    }

    public void OnClickTask(int number)
    {
<<<<<<< HEAD
        Debug.Log("numb " + number);
        //FindObjectOfType<AudioManager>().Play("buttonClick");
        currentEnc = currentEnc.PlayTurn(number);
=======
        Debug.Log(number);
        FindObjectOfType<AudioManager>().Play("buttonClick");
        curr.PlayTurn(number);
>>>>>>> f51b8ab26174d9664d6dc79a9051cb0921ab2376
        foreach(OptionButton button in buttons)
        {
            button.Destroy();
        }
        Initialize();
    }

    public void CloseDialogue()
    {
        //FindObjectOfType<AudioManager>().Play("buttonClick");
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

        /*
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
        EncounterChoice c1 = new EncounterChoice("Try to flee from the grizzly", "basic", "dex", 0.0, e.states[1], e.states[2]);
        EncounterChoice c2 = new EncounterChoice("Stand your ground and attempt to attack the grizzly", "basic", "str", 1.0, e.states[3], e.states[4]);
        e.states[0].AddChoice(c1);
        e.states[0].AddChoice(c2);
        e.states[2].AddChoice(c2);
        AddEncounter(e);
        */



<<<<<<< HEAD
        buildEncList();
        if(encList.Count > 0)
        {
            currentEnc = encList[0];
        }
        Initialize();
=======
        //Initialize();
>>>>>>> f51b8ab26174d9664d6dc79a9051cb0921ab2376
        //e.PlayEncounter();
    }


    private void buildEncList()
    {
        encList = new List<Enc>();
        ExampleNodeGraph[] node = Resources.LoadAll<ExampleNodeGraph>("StoryEncounters");
        Enc tempEnc = null;
        for (int n = 0; n < node.Length; n++)
        {
            tempEnc = (Enc)node[n].nodes[0].GetOutputPort("encOutput").GetOutputValue();
            recursiveBuildEnc(tempEnc, node, n, 0);
            encList.Add(tempEnc);
        }

        Debug.Log(encList);
    }

    private void recursiveBuildEnc(Enc currEnc, ExampleNodeGraph[] node, int nodeFileIndex, int nodeIndex)
    {
        int winNode = nodeIndex;
        int lossNode = nodeIndex;
        for (int i = 0; i < currEnc.choices.Count; i++)
        {
            currEnc.choices[i].win = (Enc)node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].winPort).GetInputValue();
            currEnc.choices[i].loss = (Enc)node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].lossPort).GetInputValue();

            if (currEnc.choices[i].win != null)
            {
                winNode = Array.IndexOf(node[nodeFileIndex].nodes.ToArray(), node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].winPort).GetConnection(0).node);
                recursiveBuildEnc(currEnc.choices[i].win, node, nodeFileIndex, winNode);
            }

            if (currEnc.choices[i].loss != null)
            {
                lossNode = Array.IndexOf(node[nodeFileIndex].nodes.ToArray(), node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].lossPort).GetConnection(0).node);
                recursiveBuildEnc(currEnc.choices[i].loss, node, nodeFileIndex, lossNode);
            }
        }
    }

}
