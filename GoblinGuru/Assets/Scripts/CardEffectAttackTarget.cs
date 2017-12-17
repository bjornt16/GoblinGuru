using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Card/AttackTarget")]
public class CardEffectAttackTarget : CardEffect
{
    public string name = "New Card Effect";
    public override string Name { get { return name; } }

    public int extraDamage = 0;

    public override void Use()
    {
        int dmg = 0 - (UnityEngine.Random.Range(player.statistics.minDamage + extraDamage, player.statistics.maxDamage + extraDamage) + player.statistics.bonusDamage);
        if(player.target != null)
        {
            player.target.Stats.ModifyHealth(dmg);
            Debug.Log("player damages target for " + dmg + " damage");
            combat.CombatUI.CombatStatusHit();
        }
    }

}
