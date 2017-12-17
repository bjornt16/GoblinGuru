using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Card/FleeCombat")]
public class CardEffectFlee : CardEffect
{
    public string name = "New Card Effect";
    public override string Name { get { return name; } }


    public override void Use()
    {
        combat.flee();
    }

}
