using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform playerTransform;
    void Update()
    {

        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);

    }
}
