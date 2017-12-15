using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[System.Serializable]
public class EncChoice
{
    public string winPort;
    public string lossPort;
    [TextArea] public string cText;
    public CharacterStats rollType = CharacterStats.D20;
    public int dc = 10;
    public EncChoiceType type;
    [System.NonSerialized]
    public Enc win;
    [System.NonSerialized]
    public Enc loss;

    public bool MustHaveItem;
    public string itemName;

    public bool MustHaveItemType;
    public CardType itemType = CardType.Consumable;

    public CombatEnemy combatTarget;

    PlayerUnit player;

    public EncChoice(string newCText, string newWinPort, string newLossPort, CharacterStats newRollType, int newDC)
    {
        winPort = newWinPort;
        lossPort = newLossPort;
        cText = newCText;
        rollType = newRollType;
        dc = newDC;

        player = GameStateManager.Instance.player;
    }

    public Enc AttemptChoice()
    {
        if(player == null)
        {
            player = GameStateManager.Instance.player;
        }

        if(type == EncChoiceType.combat)
        {
            if(combatTarget == null)
            {
                Debug.Log("Wtf?!?!");
            }
            Encounters.Instance.ui.gameObject.SetActive(false);
            Combat.Instance.Init(combatTarget);
            return win;
        }
        else if(type == EncChoiceType.none)
        {
            return win;
        }
        else
        {
            bool attempt = player.statistics.Roll(rollType, dc);
            Debug.Log(attempt);
            if (attempt)
            {
                Debug.Log("Choice Win");
                return win;
            }
            else
            {
                Debug.Log("Choice Loss");
                return loss;
            }
        }

    }
}

public enum EncChoiceType
{
    none, roll, combat
}