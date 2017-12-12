using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsUI : MonoBehaviour {

    public GameObject cardPanel;
    public GameObject cardParent;
    public GameObject cardPrefab;

    public void ToggleCardPanel()
    {
        cardPanel.SetActive(!cardPanel.activeInHierarchy);
    }
}
