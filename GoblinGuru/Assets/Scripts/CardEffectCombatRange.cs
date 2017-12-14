using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Card/Combat Range")]
public class CardEffectCombatRange : CardEffect
{
    public string name = "New range effect";
    public override string Name { get { return name; } }
    public int move;

    public override void Use()
    {
        int currentRange = (int)combat.Range;
        int nextRange = Mathf.Clamp(currentRange + move, (int)CombatRange.melee, (int)CombatRange.distant);

        combat.SetRange((CombatRange)nextRange);
        Debug.Log("player moves from" + Enum.GetName(typeof(CombatRange), currentRange) + " to " + Enum.GetName(typeof(CombatRange), nextRange));
    }

}
