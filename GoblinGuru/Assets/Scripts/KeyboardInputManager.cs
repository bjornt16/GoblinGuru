using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : MonoBehaviour {

    public PlayerUnit player;
    public GameTurnUI gameTurnUi;
    public GameObject mapUI;
    bool buttonDown = false;

    public MinimapManager mmm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(GameStateManager.Instance.GameState == GameState.Movement)
        {
            if (Input.GetButtonDown("Map"))
            {
                mapUI.SetActive(!mapUI.activeInHierarchy);
            }
            else if (Input.GetKeyDown("e"))
            {
                mmm.RotateRight();
            }
            else if (Input.GetKeyDown("q"))
            {
                mmm.RotateLeft();
            }
        }
    }
}
