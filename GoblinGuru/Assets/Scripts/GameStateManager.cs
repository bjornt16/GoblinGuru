using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public delegate void newState();
    public static event newState OnNewState;

    private static GameStateManager instance = null;
    public static GameStateManager Instance { get { return instance; } }
    public Encounters e = new Encounters();

    public GameState GameState
    {
        get
        {
            return gameState;
        }
    }

    private GameState gameState;

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


    public void startMovement()
    {
        gameState = GameState.Movement;
    }

    public void startEncounter()
    {
        gameState = GameState.Encounter;
        e.pickRandom();
    }

    public void startCombat()
    {
        gameState = GameState.Combat;
    }

}


public enum GameState{
    Movement, Encounter, Combat
}