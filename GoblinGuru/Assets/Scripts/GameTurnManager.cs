using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTurnManager : MonoBehaviour {

    public delegate void newTurn();
    public static event newTurn OnNewTurn;

    private static GameTurnManager instance = null;
    public static GameTurnManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    int turn = 1;

    public GameTurnUI gameTurnUi;

    public void Insantiate(int currentTurn)
    {
        turn = currentTurn;
    }

    public void NextTurn()
    {
        turn++;
        gameTurnUi.TurnNumber.text = turn.ToString();
        OnNewTurn();
    }

}
