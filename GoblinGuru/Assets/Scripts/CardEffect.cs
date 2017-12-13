using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject {

    public abstract string Name { get; }
    public Card cardObject;
    protected PlayerUnit player;

    public void Init(Card card)
    {
        cardObject = card;
        player = GameStateManager.Instance.player;
    }
    public abstract void Use();
}
