using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTileLabel : MonoBehaviour {

    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }


    public void Initialize(int x, int y)
    {
        name = x + "," + y;
        GetComponent<TextMeshProUGUI>().text = "x:"+ x + "\n" + "y:" + y;
    }
}
