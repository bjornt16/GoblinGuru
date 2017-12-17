﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class Encounters : MonoBehaviour {

    private static Encounters instance = null;
    public static Encounters Instance { get { return instance; } }

    public PlayerUnit player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public EncounterUI ui;

    List<Enc> randEncList;
    List<Enc> encList;
    List<Encounter> encounters;
    List<OptionButton> buttons;

    int encIndex = 0;
    public int CombatChoiceNumber;

    Enc currentEnc;
    Enc currentRandEnc;

    List<Encounter> randomEncounters;

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
        currentEnc = randEncList[0];
        Initialize();
    }

    public void Initialize()
    {
        if(player == null)
        {
            player = GameStateManager.Instance.player;
        }
        buttons = new List<OptionButton>();

        ui.stateText.text = currentEnc.text;

        if(currentEnc.choices.Count == 0)
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
                if((currentEnc.choices[i].MustHaveItem && player.HasCard(currentEnc.choices[i].itemName)) || !currentEnc.choices[i].MustHaveItem ||
                    (currentEnc.choices[i].MustNotHaveItem && !player.HasCard(currentEnc.choices[i].itemName)))
                {


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

        */


        ui.gameObject.SetActive(true);

    }

    public void CombatOver()
    {
        foreach (OptionButton button in buttons)
        {
            button.Destroy();
        }

        Initialize();
    }

    public void OnClickTask(int number)
    {
        if(currentEnc.choices[number].type == EncChoiceType.combat)
        {
            CombatChoiceNumber = number;
        }

        if (currentEnc.choices[number].cText == "Try Again?")
        {
            currentEnc = encList[2];
            FindObjectOfType<AudioManager>().Play("buttonClick");

            foreach (OptionButton button in buttons)
            {
                button.Destroy();
            }
            Initialize();
        }
        else
        {
            Debug.Log("numb " + number);
            currentEnc = currentEnc.PlayTurn(number);

            FindObjectOfType<AudioManager>().Play("buttonClick");

            foreach (OptionButton button in buttons)
            {
                button.Destroy();
            }


            Initialize();
        }

    }

    public void CloseDialogue()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");
        //todo functionality
        encIndex++;
        if (encIndex < 4)
        {
            currentEnc = encList[encIndex];
            foreach (OptionButton button in buttons)
            {
                button.Destroy();
            }
            Initialize();
        }
        else
        {
            ui.gameObject.SetActive(false);
            foreach (OptionButton button in buttons)
            {
                button.Destroy();
            }
            GameStateManager.Instance.startMovement();
        }
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


        encList = buildEncList("StoryEncounters");
        if(encList.Count > 0)
        {
            encIndex = 0;
            currentEnc = encList[0];
        }
        randEncList = buildEncList("RandomStoryEncounters");
        currentRandEnc = randEncList[0];
       
        Initialize();

        //Initialize();
        //e.PlayEncounter();
    }


    private List<Enc> buildEncList(string folder)
    {
        List<Enc> list = new List<Enc>();
        List<Enc> tempEncList = new List<Enc>();
        EncounterNodeGraph[] node = Resources.LoadAll<EncounterNodeGraph>(folder);
        Enc tempEnc = null;

        for (int n = 0; n < node.Length; n++)
        {
            Debug.Log("name " + node[n].name);
            if(node[n].nodes.Count != 0)
            {
                tempEnc = (Enc)node[n].nodes[0].GetOutputPort("encOutput").GetOutputValue();
                recursiveBuildEnc(tempEnc, node, n, 0, tempEncList);
                list.Add(tempEnc);
            }
        }

        Debug.Log(list);
        return list;
    }

    private Enc CheckForDuplicate(List<Enc> list, Enc enc)
    {
        bool same = false;
        if(enc != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                same = list[i].text == enc.text;
                if (!same)
                {
                    continue;
                }
                same = list[i].choices.Count == enc.choices.Count;
                if (same)
                {
                    for (int l = 0; l < list[i].choices.Count; l++)
                    {
                        same = list[i].choices[l].cText == enc.choices[l].cText;
                        if (!same)
                        {
                            break;
                        }

                    }
                }

                if (same)
                {
                    return list[i];
                }

            }
        }
        return null;
    }

    private void recursiveBuildEnc(Enc currEnc, EncounterNodeGraph[] node, int nodeFileIndex, int nodeIndex, List<Enc> tempEncList)
    {
        int winNode = nodeIndex;
        int lossNode = nodeIndex;
        for (int i = 0; i < currEnc.choices.Count; i++)
        {
            currEnc.choices[i].win = (Enc)node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].winPort).GetInputValue();
            currEnc.choices[i].loss = (Enc)node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].lossPort).GetInputValue();

            Enc duplicateWin = CheckForDuplicate(tempEncList, currEnc.choices[i].win);
            Enc duplicateLoss = CheckForDuplicate(tempEncList, currEnc.choices[i].loss);

            Debug.Log("dup: " + duplicateWin);

            if (currEnc.choices[i].win != null && duplicateWin == null)
            {
                tempEncList.Add(currEnc.choices[i].win);
                winNode = Array.IndexOf(node[nodeFileIndex].nodes.ToArray(), node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].winPort).GetConnection(0).node);
                recursiveBuildEnc(currEnc.choices[i].win, node, nodeFileIndex, winNode, tempEncList);
            }
            else
            {
                currEnc.choices[i].win = duplicateWin;
            }

            if (currEnc.choices[i].loss != null && duplicateLoss == null)
            {
                tempEncList.Add(currEnc.choices[i].loss);
                lossNode = Array.IndexOf(node[nodeFileIndex].nodes.ToArray(), node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].lossPort).GetConnection(0).node);
                recursiveBuildEnc(currEnc.choices[i].loss, node, nodeFileIndex, lossNode, tempEncList);
            }
            else
            {
                currEnc.choices[i].loss = duplicateLoss;
            }
        }
    }

}
