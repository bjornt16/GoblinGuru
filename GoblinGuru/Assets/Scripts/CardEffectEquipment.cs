using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Card/Equipment Effect")]
public class CardEffectEquipment : CardEffect
{
    public string name = "New Card Effect";
    public override string Name { get { return name; } }

    public override void Use()
    {
        if (cardObject.type == CardType.Equipment)
        {
            if (!cardObject.Equipped)
            {
                if (cardObject.Slot == CardEquipmentSlot.Head)
                {
                    if(player.Head != null)
                    {
                        //todo unequip
                        player.Head.OnUse();
                    }
                    player.Head = cardObject;
                }
                else if(cardObject.Slot == CardEquipmentSlot.Torso)
                {
                    if (player.Torso != null)
                    {
                        //todo unequip
                        player.Torso.OnUse();
                    }
                    player.Torso = cardObject;
                }
                else if (cardObject.Slot == CardEquipmentSlot.Legs)
                {
                    if (player.Legs != null)
                    {
                        //todo unequip
                        player.Legs.OnUse();
                    }
                    player.Legs = cardObject;
                }
                else if (cardObject.Slot == CardEquipmentSlot.Feet)
                {
                    if (player.Feet != null)
                    {
                        //todo unequip
                        player.Feet.OnUse();
                    }
                    player.Feet = cardObject;
                }
                cardObject.Equipped = true;
            }
            else
            {
                if (cardObject.Slot == CardEquipmentSlot.Head)
                {
                    player.Head = null;
                }
                else if (cardObject.Slot == CardEquipmentSlot.Torso)
                {
                    player.Torso = null;
                }
                else if (cardObject.Slot == CardEquipmentSlot.Legs)
                {
                    player.Legs = null;
                }
                else if (cardObject.Slot == CardEquipmentSlot.Feet)
                {
                    player.Feet = null;
                }
                cardObject.Equipped = false;
            }
        }
        else
        {
            Debug.Log("card not equipment type");
        }
    }

}
