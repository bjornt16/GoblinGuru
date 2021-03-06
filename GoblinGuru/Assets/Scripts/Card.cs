﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour {

    public string cardName;
    public UnityEngine.Sprite image;
    
    public string description;
    public int charges;

    public CardUsable[] UsableIn;
    public CombatRange[] UsableRange;
    public CardType type;

    public CardEffect effect;
    private CardEffect effectInstance;

    public UnityEngine.UI.Button cardClicker;

    public UnityEngine.UI.Image cardImage;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDescriptionText;
    public TextMeshProUGUI cardStaminaCostText;
    public TextMeshProUGUI cardStat;

    public GameObject blockImage;
    public GameObject EquippedBorder;

    private bool equipped = false;
    public CardEquipmentSlot Slot = CardEquipmentSlot.None;

    public bool Equipped
    {
        get
        {
            return equipped;
        }

        set
        {
            equipped = value;
            if(EquippedBorder != null) {
                EquippedBorder.SetActive(equipped);
            }
    }
    }

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

        Equipped = cloneFrom.Equipped;
        Slot = cloneFrom.Slot;

        SetUI();
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

        Equipped = cloneFrom.Equipped;
        Slot = cloneFrom.Slot;
    }

    private void SetUI()
    {
        if(gameObject != null)
        {
            cardNameText.text = cardName;
            cardDescriptionText.text = description;
            cardStaminaCostText.text = "0";
            cardStat.text = "0";
            if(image != null)
            {
                cardImage.sprite = image;
            }
            
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnUse()
    {
        if(effect != null)
        {
            if (effectInstance == null)
            {
                effectInstance = Instantiate(effect);
            }
            if (effectInstance.cardObject == null)
            {
                effectInstance.Init(this);
            }
            effectInstance.Use();
        }

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
    Consumable, Equipment, Spell, Attack, Special, Movement
}

public enum CardEquipmentSlot
{
    None, Head, Torso, Legs, Feet
}