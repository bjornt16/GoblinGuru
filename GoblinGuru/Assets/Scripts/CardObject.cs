using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/CardObject")]
public class CardObject : ScriptableObject {

    public string cardName;
    public UnityEngine.UI.RawImage image;
    public string description;
    public int charges;

    public CardUsable[] UsableIn;
    public CombatRange[] UsableRange;
    public CardType type;

    public CardEffect effect;

    public bool Equipped = false;
    public CardEquipmentSlot Slot = CardEquipmentSlot.None;

}
