using System;
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
    List<OptionButton> buttons;

    int encIndex = 0;
    public int CombatChoiceNumber;

    Enc currentEnc;
    Enc currentRandEnc;

    public string firstMainStoryEncounterName;
    public List<Enc> openMainStory;

    public GameObject questionMarkPrefab;

    public Encounters()
    {

    }
    
    public void AddEncounter(Encounter enc)
    {

    }

    public Map map;

    public void pickRandom()
    {
        if(player.Tile.Encounter != null)
        {
            currentEnc = player.Tile.Encounter;
            player.Tile.Encounter = null;
            DestroyObject(player.Tile.EncQuestionMark);
            player.Tile.EncQuestionMark = null;
        }
        else
        {
            Debug.Log("getRandom");
            currentEnc = GetRandomStoryEncounter();
        }
        Debug.Log(currentEnc.name + " " + player.Tile);
        Initialize();
    }

    public void Initialize()
    {
        if(player == null)
        {
            player = GameStateManager.Instance.player;
        }

        if(currentEnc != null)
        {
            ui.EncounterImg.sprite = currentEnc.image;
        }
        

        if (currentEnc.hasCost)
        {
            player.statistics.ModifyHealth(-currentEnc.hpCost);
            player.statistics.ModifyStamina(-currentEnc.staminaCost);
            player.Gold -= currentEnc.goldCost;

            //todo cost item
        }
        if (currentEnc.hasReward)
        {
            player.statistics.ModifyHealth(currentEnc.hpReward);
            player.statistics.ModifyStamina(currentEnc.staminaReward);
            player.Gold += currentEnc.goldReward;

            Debug.Log(currentEnc.rollSleep);
            if (currentEnc.rollSleep != SleepType.none)
            {
                player.statistics.DoSleep(currentEnc.rollSleep);
            }
            //todo reward item
        }

        if(currentEnc.hasCost || currentEnc.hasReward)
        {
            player.UpdateUI();
        }

        if (currentEnc.hasVariable)
        {
            player.EncounterVariable[currentEnc.variableKey] = currentEnc.variableValue;
            Debug.Log(player.EncounterVariable);
        }

        buttons = new List<OptionButton>();

        ui.stateText.text = currentEnc.text;

        if(currentEnc.choices.Count == 0 )
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
                if(((currentEnc.choices[i].MustHaveItem && player.HasCard(currentEnc.choices[i].itemName)) ||
                    (currentEnc.choices[i].MustNotHaveItem && !player.HasCard(currentEnc.choices[i].itemName))) || (currentEnc.choices[i].mustHaveVariable &&
                        (player.EncounterVariable.ContainsKey(currentEnc.choices[i].variableKey) &&
                        player.EncounterVariable[currentEnc.choices[i].variableKey] == currentEnc.choices[i].variableValue)) ||
                         (!currentEnc.choices[i].MustHaveItem && !currentEnc.choices[i].mustHaveVariable))
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
        List<string> openList = currentEnc.encounters;
        bool startRightAway = false;
        for (int i = 0; i < openList.Count; i++)
        {
            Enc temp = GetEncounterByName(encList, openList[i]);
            if(temp != null && CheckForDuplicate(openMainStory, temp) == null)
            {
                openMainStory.Add(GetEncounterByName(encList, openList[i]));
            }
            if (currentEnc.triggerOnComplete != null && i < currentEnc.triggerOnComplete.Count && currentEnc.triggerOnComplete[i])
            {
                currentEnc = GetEncounterByName(encList, openList[i]);
                startRightAway = true;
            }
            else if(temp != null)
            {
                //todo spawn stuff
                GameTile spawn = GetNearestTile(temp.terrainType, temp.featureType, 3);

                spawn.Encounter = temp;
                spawn.EncQuestionMark = Instantiate(questionMarkPrefab);
                spawn.EncQuestionMark.transform.position = spawn.transform.position;
            }
        }

        foreach (OptionButton button in buttons)
        {
            button.Destroy();
        }

        if (startRightAway)
        {
            Initialize();
        }
        else
        {

            ui.gameObject.SetActive(false);
            GameStateManager.Instance.startMovement();
        }
        
    }

    // Use this for initialization
    void Start()
    {

        LoadAllEncounters();


        Initialize();
    }

    private void LoadAllEncounters()
    {
        encList = buildEncList("StoryEncounters");
        if (encList.Count > 0)
        {
            encIndex = 0;
            currentEnc = GetEncounterByName(encList, firstMainStoryEncounterName);
        }
        randEncList = buildEncList("RandomStoryEncounters");
        currentRandEnc = randEncList[0];
    }


    private Enc GetRandomStoryEncounter()
    {
        int randIndex = (int)UnityEngine.Random.Range(0, randEncList.Count);
        Debug.Log("randindex" + randIndex);
        return randEncList[randIndex];
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
                if(tempEnc != null)
                {
                    tempEnc.name = node[n].name;
                    tempEnc.image = node[n].image;
                    Debug.Log(tempEnc);
                    recursiveBuildEnc(tempEnc, node, n, 0, tempEncList, tempEnc.image);
                    list.Add(tempEnc);
                }
            }
        }

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

    private void recursiveBuildEnc(Enc currEnc, EncounterNodeGraph[] node, int nodeFileIndex, int nodeIndex, List<Enc> tempEncList, UnityEngine.Sprite img)
    {
        if(img != null)
        {
            Debug.Log(currentEnc + " ss " + img);
            currEnc.image = img;
        }

        int winNode = nodeIndex;
        int lossNode = nodeIndex;
        for (int i = 0; i < currEnc.choices.Count; i++)
        {
            currEnc.choices[i].win = (Enc)node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].winPort).GetInputValue();
            currEnc.choices[i].loss = (Enc)node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].lossPort).GetInputValue();

            Enc duplicateWin = CheckForDuplicate(tempEncList, currEnc.choices[i].win);
            Enc duplicateLoss = CheckForDuplicate(tempEncList, currEnc.choices[i].loss);

            if (currEnc.choices[i].win != null && duplicateWin == null)
            {
                tempEncList.Add(currEnc.choices[i].win);
                winNode = Array.IndexOf(node[nodeFileIndex].nodes.ToArray(), node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].winPort).GetConnection(0).node);
                recursiveBuildEnc(currEnc.choices[i].win, node, nodeFileIndex, winNode, tempEncList, img);
            }
            else
            {
                currEnc.choices[i].win = duplicateWin;
            }

            if (currEnc.choices[i].loss != null && duplicateLoss == null)
            {
                tempEncList.Add(currEnc.choices[i].loss);
                lossNode = Array.IndexOf(node[nodeFileIndex].nodes.ToArray(), node[nodeFileIndex].nodes[nodeIndex].GetInputPort(currEnc.choices[i].lossPort).GetConnection(0).node);
                recursiveBuildEnc(currEnc.choices[i].loss, node, nodeFileIndex, lossNode, tempEncList, img);
            }
            else
            {
                currEnc.choices[i].loss = duplicateLoss;
            }
        }
    }

    private Enc GetEncounterByName(List<Enc> list, string name)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].name == name)
            {
                return list[i];
            }
        }
        return null;
    }

    public GameTile GetNearestTile(TileTerrain terrain, TileFeatures feature, int distance)
    {
        return map.GetNearestTile(player.Tile, terrain, feature, distance);
    }

}
