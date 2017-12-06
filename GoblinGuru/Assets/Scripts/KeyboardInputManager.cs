using System.Collections;
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

        if (!buttonDown)
        {
            if (Input.GetKeyDown("up") || Input.GetAxis("Vertical") < 0 )
            {
                player.goUp();
                buttonDown = true;
            }

            if (Input.GetKeyDown("down") || Input.GetAxis("Vertical") > 0 )
            {
                player.goDown();
                buttonDown = true;
            }

            if (Input.GetKeyDown("left") || Input.GetAxis("Horizontal") < 0)
            {
                player.goLeft();
                buttonDown = true;
            }

            if (Input.GetKeyDown("right") || Input.GetAxis("Horizontal") > 0)
            {
                player.goRight();
                buttonDown = true;
            }

            if (Input.GetKeyDown("space"))
            {
                gameTurnUi.EncounterButton.onClick.Invoke();
                buttonDown = true;
            }
        }
        else if (buttonDown && !player.Moving && !Input.anyKeyDown && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            buttonDown = false;
        }
    }
}
