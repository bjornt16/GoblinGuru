using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

    public CombatUI CombatUI;
    public CombatViewModel CombatViewModelPrefab;
    public Card CombatViewCardPrefab;
    public PlayerUnit player;
    public CombatEnemy target;
    private CombatEnemy targetInstance;

    public CombatViewModel playerModel;
    public CombatViewModel targetModel;

    private List<Card> cardList;

    bool isPlayerTurn = false;
    int combatTurn;

    int consumableCount = 0;

    public void Init()//CombatEnemy targetObject)
    {
        if(targetInstance == null)
        {
            targetInstance = Instantiate(target);
        }

        playerModel = Instantiate(CombatViewModelPrefab, CombatUI.GroundMelee[0].transform);
        targetModel = Instantiate(CombatViewModelPrefab, CombatUI.GroundMelee[1].transform);
        targetModel.Init(targetInstance.HeadColor, targetInstance.TorsoColor, targetInstance.LegsColor, targetInstance.FeetColor);

        player.target = target;

        getPlayerCards();
    }

    private void getPlayerCards()
    {
        cardList = new List<Card>();
        for (int i = 0; i < player.cardList.Count; i++)
        {
            cardList.Add(Instantiate(CombatViewCardPrefab, CombatUI.CardParent.transform));
            cardList[i].CloneValueFrom(player.cardList[i]);
            CardType tempType = cardList[i].type;
            cardList[i].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ActionTaken(tempType));
        }
    }

    public void ActionTaken(CardType type)
    {
        if(type == CardType.Consumable && consumableCount == 0)
        {
            consumableCount++;
            Debug.Log("Consumable");
        }
        else
        {
            consumableCount = 0;
            Debug.Log("Next Turn");
        }

        CombatUI.UpdateUI(player.statistics, target.Stats);
    }


    private void Start()
    {
        Init();
    }

    public bool RollStrength(CharStats player, CharStats target)
    {
        //todo roll random d20 + stat against target?
        return false;
    }
}


public enum CombatRange
{
    melee, ranged, distant
}