using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {
    public delegate void newState();
    public static event newState OnNewRound;

    private static Combat instance = null;
    public static Combat Instance { get { return instance; } }

    public int CombatTurn
    {
        get
        {
            return combatTurn;
        }
    }

    public CombatRange Range
    {
        get
        {
            return range;
        }
    }

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
    CombatRange range = CombatRange.melee;

    public void Init()//CombatEnemy targetObject)
    {
        if(targetInstance == null)
        {
            targetInstance = Instantiate(target);
        }

        playerModel = Instantiate(CombatViewModelPrefab, CombatUI.GroundMelee[0].transform);
        targetModel = Instantiate(CombatViewModelPrefab, CombatUI.GroundMelee[1].transform);
        targetModel.Init(targetInstance.HeadColor, targetInstance.TorsoColor, targetInstance.LegsColor, targetInstance.FeetColor);

        player.target = targetInstance;

        getPlayerCards();
    }

    public void SetRange(CombatRange r)
    {
        range = r;
        if(range == CombatRange.melee)
        {
            CombatUI.SetRangeMelee(playerModel, targetModel);
        }
        else if (range == CombatRange.ranged)
        {
            CombatUI.SetRangeRanged(playerModel, targetModel);
        }
        else if (range == CombatRange.distant)
        {
            CombatUI.SetRangeDistant(playerModel, targetModel);
        }
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
            isPlayerTurn = !isPlayerTurn;
            SetPlayerCardsActive(false);
            combatTurn++;
        }

        CombatUI.combatRoundText.text = CombatTurn.ToString();
        CombatUI.UpdateUI(player.statistics, targetInstance.Stats);
        if (CheckCombatEnd())
        {
            CombatUI.CombatEnd(player.statistics, targetInstance.Stats);
        }
        CombatUI.SetTargetTurn();
        targetInstance.DoCombatAction();
        SetPlayerCardsActive(true);
        isPlayerTurn = true;
    }

    private bool CheckCombatEnd()
    {
        return player.statistics.HP <= 0 || targetInstance.Stats.HP <= 0;
    }

    public void EndCombat()
    {

    }

    private void SetPlayerCardsActive(bool set)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if(cardList[i] != null)
            {
                cardList[i].cardClicker.enabled = set;
            }
            
        }
    }


    private void Start()
    {
        Init();
    }

}


public enum CombatRange
{
    melee, ranged, distant
}