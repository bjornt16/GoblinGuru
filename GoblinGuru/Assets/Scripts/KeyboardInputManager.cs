﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : MonoBehaviour {

    public PlayerUnit player;
    public GameTurnUI gameTurnUi;
    bool buttonDown = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if ((!buttonDown && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f)) || (
            buttonDown && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.8f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.8f)))
        {
            if (Input.GetKeyDown("up") || Input.GetAxis("Vertical") < 0 )
            {
                player.GoUp();
                buttonDown = true;
            }

            if (Input.GetKeyDown("down") || Input.GetAxis("Vertical") > 0 )
            {
                player.GoDown();
                buttonDown = true;
            }

            if (Input.GetKeyDown("left") || Input.GetAxis("Horizontal") < 0)
            {
                player.GoLeft();
                buttonDown = true;
            }

            if (Input.GetKeyDown("right") || Input.GetAxis("Horizontal") > 0)
            {
                player.GoRight();
                buttonDown = true;
            }
        }
        else if (buttonDown && !player.Moving && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f && Mathf.Abs(Input.GetAxis("Vertical")) < 0.1f)
        {
            buttonDown = false;
        }
        else if (!player.Moving && Input.GetKeyDown("space"))
        {
            gameTurnUi.EncounterButton.onClick.Invoke();
            buttonDown = true;
        }
    }
}
