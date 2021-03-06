﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public delegate void newState();
    public static event newState OnNewState;

    private static GameStateManager instance = null;
    public static GameStateManager Instance { get { return instance; } }
    public Encounters e = new Encounters();

    public PlayerUnit player;

    public GameObject dingUI;

    public GameObject gameOverUI;

    private void Start()
    {
        gameState = GameState.Encounter;
    }

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
        player.statistics.currXp += 1;
        int level = player.statistics.level;
        player.checkForDing();
        if (level != player.statistics.level)
        {
            dingUI.SetActive(true);
            FindObjectOfType<AudioManager>().Play("ding");
        }

        gameState = GameState.Movement;
    }
    
    //takes input base on if encounter or rest button is pressed
    public void startEncounter(string name)
    {
        FindObjectOfType<AudioManager>().Play("enterEnc");
        GameTurnManager.Instance.NextTurn();
        gameState = GameState.Encounter;
        e.pickRandom();
    }

    public void startRestEncounter(string name)
    {
        FindObjectOfType<AudioManager>().Play("enterEnc");
        GameTurnManager.Instance.NextTurn();
        gameState = GameState.Encounter;
        e.pickRandomRest();
    }

    public void startCombat()
    {
        gameState = GameState.Combat;
    }

    public void startGameOver()
    {
        gameState = GameState.GameOver;
        gameOverUI.SetActive(true);
    }

    public void reloadScene()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

}


public enum GameState{
    Movement, Encounter, Combat, GameOver
}