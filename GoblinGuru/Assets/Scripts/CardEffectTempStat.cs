using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Card/StatBoostEffect")]
public class CardEffectTempStat : CardEffect
{
    public string name = "New Card Effect";
    public override string Name { get { return name; } }

    public string statName;
    public int statIncrease;
    public int turnDuration;

    public override void Use()
    {
        Debug.Log("players " + statName + " stat increases by " + statIncrease + " for " + turnDuration);
        cardObject.charges--;
        if(cardObject.charges <= 0)
        {
            cardObject.Destroy();
        }
    }

}
