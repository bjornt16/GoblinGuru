using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour {

    Vector3 location;

    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }

}
