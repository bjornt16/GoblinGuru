using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsUI : MonoBehaviour {

    public GameObject cardPanel;
    public GameObject cardParent;
    public Card cardPrefab;

    public PlayerUnit player;
    private List<Card> cardList;

    public void ToggleCardPanel()
    {
        cardPanel.SetActive(!cardPanel.activeInHierarchy);
    }

    private void Start()
    {
        InstantiatePlayerCards();
    }

    private void InstantiatePlayerCards()
    {
        Card tempCard;
        cardList = new List<Card>();
        for (int i = 0; i < player.cardList.Count; i++)
        {
            tempCard = Instantiate(cardPrefab, cardParent.transform);
            tempCard.CloneValueFrom(player.cardList[i]);
            cardList.Add(tempCard);
        }
    }

    private void SetActiveAllCards(bool set)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if(cardList[i] == null)
            {
                cardList.RemoveAt(i);
            }
            else
            {
                cardList[i].gameObject.SetActive(set);
            }
        }
    }

    private void ShowEquipped()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i] == null)
            {
                cardList.RemoveAt(i);
            }
            else if(cardList[i].Equipped)
            {
                cardList[i].gameObject.SetActive(true);
            }
        }
    }
    private void ShowCardByType(CardType type)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i] == null)
            {
                cardList.RemoveAt(i);
            }
            else if (cardList[i].type == type)
            {
                cardList[i].gameObject.SetActive(true);
            }
        }
    }

    public void CardsByType(int type)
    {
        if (type == 0)
        {
            SetActiveAllCards(true);
        }
        else
        {
            SetActiveAllCards(false);
        }

        if(type == 1)
        {
            ShowEquipped();
        }
        else if (type == 2)
        {
            ShowCardByType(CardType.Equipment);
        }
        else if (type == 3)
        {
            ShowCardByType(CardType.Consumable);
        }
        else if (type == 4)
        {
            ShowCardByType(CardType.Spell);
        }
        else if (type == 5)
        {
            ShowCardByType(CardType.Attack);
        }
        else if (type == 6)
        {
            ShowCardByType(CardType.Movement);
        }
        else if (type == 7)
        {
            ShowCardByType(CardType.Special);
        }
    }
}
