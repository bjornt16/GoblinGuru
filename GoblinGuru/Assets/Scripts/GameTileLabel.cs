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
        name = x + ",\n " + y;
        GetComponent<TextMeshProUGUI>().text = name;
    }
}
