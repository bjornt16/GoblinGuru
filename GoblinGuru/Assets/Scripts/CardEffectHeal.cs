using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Card/HealEffect")]
public class CardEffectHeal : CardEffect
{
    public string name = "New Card Effect";
    public override string Name { get { return name; } }
    public int healMin = 5;
    public int healMax = 10;

    public override void Use()
    {
        System.Random rnd = new System.Random();
        int heal = healMin < 0 ? rnd.Next(healMax, healMin - 1) : rnd.Next(healMin, healMax + 1);
        player.statistics.ModifyHealth(heal);   
        Debug.Log("player heals for " + heal);
        cardObject.charges--;
        if(cardObject.charges == 0)
        {
            cardObject.Destroy();
        }
    }

}
