using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "CardEffect/Potion")]
public class CardEffectHeal : CardEffect
{
    public string name = "New Card Effect";
    public override string Name { get { return name; } }
    public int healMin = 5;
    public int healMax = 10;

    public override void Use()
    {
        System.Random rnd = new System.Random();
        Debug.Log("player heals for " + rnd.Next(healMin, healMax + 1));
        cardObject.charges--;
        if(cardObject.charges == 0)
        {
            cardObject.Destroy();
        }
    }

}
