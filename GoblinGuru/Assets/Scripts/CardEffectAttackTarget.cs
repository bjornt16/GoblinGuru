using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Card/AttackTarget")]
public class CardEffectAttackTarget : CardEffect
{
    public string name = "New Card Effect";
    public override string Name { get { return name; } }

    public override void Use()
    {
        System.Random rnd = new System.Random();
        int dmg = 0 - (UnityEngine.Random.Range(player.statistics.minDamage, player.statistics.maxDamage) + player.statistics.bonusDamage);
        if(player.target != null)
        {
            player.target.Stats.ModifyHealth(dmg);
            Debug.Log("player damages target for " + dmg + " damage");
            combat.CombatUI.CombatStatusHit();
        }
    }

}
