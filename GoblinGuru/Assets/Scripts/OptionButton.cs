using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionButton : MonoBehaviour {

    public int parameter;
    public string stateText;
    public UnityEngine.UI.Button option;
    public TextMeshProUGUI uText;
    

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Initialize(int number, string text)
    {
        parameter = number;
        stateText = text;
        uText.text = stateText;
    }
    
}
