using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public string cardName;
    public UnityEngine.UI.RawImage image;
    public string description;
    public int charges;

    public CardEffect effect;
    private CardEffect effectInstance;

    int staminaCost;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnUse()
    {
        if(effectInstance == null)
        {
            effectInstance = Instantiate(effect);
        }
        effectInstance.Init(this);
        effectInstance.Use();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
