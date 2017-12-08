using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enc {

    public string text;

    public List<EncChoice> choices = new List<EncChoice>();

    public Enc(string t, List<EncChoice> c)
    {
        text = t;
        choices = c;
    }


    public void PlayEncounter()
    {

        PlayTurn();
    }

    public Enc PlayTurn(int choice)
    {
        Debug.Log(choices[choice].cText);


        return choices[choice].AttemptChoice();

    }

    public void PlayTurn()
    {
        Debug.Log(text);
        for (int i = 0; i < choices.Count; i++)
        {
            Debug.Log(i + " - " + choices[i].cText);
        }
        int numChoice = 0;
        choices[numChoice].AttemptChoice();
    }
}
