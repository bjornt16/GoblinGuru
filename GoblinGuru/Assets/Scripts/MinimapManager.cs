using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{

    public Transform player;

    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }

    public void ZoomIn()
    {
        if (GetComponent<Camera>().orthographicSize > 4)
        {
            GetComponent<Camera>().orthographicSize -= 2;
        }
    }

    public void ZoomOut()
    {

        if (GetComponent<Camera>().orthographicSize < 22)
        {
            GetComponent<Camera>().orthographicSize += 2;
        }
    }

}
