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

    public void Init(CombatEnemy targetObject)
    {
        target = targetObject;
        targetInstance = Instantiate(target);

        CombatUI.combatOverPanel.SetActive(false);
        CombatUI.CardPanel.gameObject.SetActive(true);
        CombatUI.gameObject.SetActive(true);

        playerModel = Instantiate(CombatViewModelPrefab, CombatUI.GroundMelee[0].transform);
        targetModel = Instantiate(CombatViewModelPrefab, CombatUI.GroundMelee[1].transform);
        targetModel.Init(targetInstance.HeadColor, targetInstance.TorsoColor, targetInstance.LegsColor, targetInstance.FeetColor);

        player.target = targetInstance;

        range = CombatRange.melee;
        consumableCount = 0;
        combatTurn = 0;

        CombatUI.combatRoundText.text = CombatTurn.ToString();
        CombatUI.combatStatusText.text = "";

        CombatUI.UpdateUI(player.statistics, target.Stats);

        getPlayerCards();
    }

    public void SetRange(CombatRange r)
    {
        range = r;
        CardsByRange(r);
        if (range == CombatRange.melee)
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

    private void CardsByRange(CombatRange range)
    {
        bool isRange;
        for (int i = 0; i < cardList.Count; i++)
        {
            isRange = false;
            for (int k = 0; k < cardList[i].UsableRange.Length; k++)
            {
                if(cardList[i].UsableRange[k] == range)
                {
                    isRange = true;
                    cardList[i].gameObject.SetActive(true);
                    break;
                }
            }
            if (!isRange)
            {
                cardList[i].gameObject.SetActive(false);
            }
        }
    }

    private void ClearCardList()
    {
        if(cardList != null)
        {
            for (int i = 0; i < player.cardList.Count; i++)
            {
                cardList[i].Destroy();
            }
        }
        cardList = new List<Card>();
    }

    private void getPlayerCards()
    {
        ClearCardList();
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
        }
        CombatUI.UpdateUI(player.statistics, targetInstance.Stats);
        CombatUI.SetTargetTurn();
        if (CheckCombatEnd())
        {
            CombatUI.CombatEnd(player.statistics, targetInstance.Stats);
            CombatUI.combatRoundText.text = "Combat Over!";
            CombatUI.CardPanel.gameObject.SetActive(false);
            CombatUI.combatOverPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(SimulateTargetRound());
        }
    }

    IEnumerator SimulateTargetRound()
    {
        float elapsedTime = 0;
        while (elapsedTime < 2)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        combatTurn++;
        CombatUI.combatRoundText.text = CombatTurn.ToString();
        targetInstance.DoCombatAction();
        SetPlayerCardsActive(true);
        CombatUI.SetPlayerTurn();
        CombatUI.combatStatusText.text = "";
        isPlayerTurn = true;
    }

    private bool CheckCombatEnd()
    {
        return player.statistics.HP <= 0 || targetInstance.Stats.HP <= 0;
    }

    public void EndCombat()
    {
        CombatUI.gameObject.SetActive(false);
        Encounters.Instance.CombatOver();
    }

    private void SetPlayerCardsActive(bool set)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if(cardList[i] != null)
            {
                cardList[i].cardClicker.enabled = set;
                if (set)
                {
                    cardList[i].blockImage.SetActive(false);
                }
                else
                {
                    cardList[i].blockImage.SetActive(true);
                }
            }
            
        }
    }


    private void Start()
    {
        //Init(target);
    }

}


public enum CombatRange
{
    melee, ranged, distant
}