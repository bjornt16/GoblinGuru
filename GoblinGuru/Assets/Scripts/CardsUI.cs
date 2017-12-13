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
        for (int i = 0; i < player.cardList.Count; i++)
        {
            tempCard = Instantiate(cardPrefab, cardParent.transform);
            tempCard.CloneValueFrom(player.cardList[i]);
        }
    }
}
