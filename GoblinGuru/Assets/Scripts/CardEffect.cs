using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject {

    public abstract string Name { get; }
    public Card cardObject;

    public void Init(Card card)
    {
        cardObject = card;
    }
    public abstract void Use();
}
