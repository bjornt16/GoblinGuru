using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public string cardName;
    public UnityEngine.UI.RawImage image;
    public string description;
    public int charges;

    public CardUsable[] UsableIn;
    public CombatRange[] UsableRange;
    public CardType type;

    public CardEffect effect;
    private CardEffect effectInstance;

    public UnityEngine.UI.Button cardClicker;

    public void CloneValueFrom(Card cloneFrom)
    {
        cardName = cloneFrom.cardName;
        image = cloneFrom.image;
        description = cloneFrom.description;
        charges = cloneFrom.charges;
        effect = cloneFrom.effect;

        UsableIn = cloneFrom.UsableIn;
        UsableRange = cloneFrom.UsableRange;
        type = cloneFrom.type;
    }

    public void CloneValueFrom(CardObject cloneFrom)
    {
        cardName = cloneFrom.cardName;
        image = cloneFrom.image;
        description = cloneFrom.description;
        charges = cloneFrom.charges;
        effect = cloneFrom.effect;

        UsableIn = cloneFrom.UsableIn;
        UsableRange = cloneFrom.UsableRange;
        type = cloneFrom.type;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnUse()
    {
        if(effectInstance == null)
        {
            effectInstance = Instantiate(effect);
        }
        if(effectInstance.cardObject == null)
        {
            effectInstance.Init(this);
        }
        effectInstance.Use();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}


public enum CardUsable
{
    Movement, Encounter, Combat
}

public enum CardType
{
    Consumable, Equipment, Spell, Attack, Special
}