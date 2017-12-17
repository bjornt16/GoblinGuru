using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{

    public Transform player;

    public GameObject camera;

    private bool rotating;

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

    public void RotateRight()
    {
        if (!rotating)
        {
            rotating = true;
            Vector3 tempRotation = camera.transform.localEulerAngles;
            Vector3 oldRotation = tempRotation;
            tempRotation.y += 90f;

            Vector3 tempPosition = camera.transform.localPosition;
            Vector3 oldPosition = tempPosition;
            Debug.Log(tempRotation);


            camera.transform.localEulerAngles = tempRotation;
            camera.transform.localPosition = tempPosition;
            StartCoroutine(AnimateRotation(oldRotation, oldPosition, tempRotation, tempPosition));
        }
    }

    public void RotateLeft()
    {
        if (!rotating)
        {
            rotating = true;
            Vector3 tempRotation = camera.transform.localEulerAngles;
            Vector3 oldRotation = tempRotation;
            tempRotation.y += -90f;

            Vector3 tempPosition = camera.transform.localPosition;
            Vector3 oldPosition = tempPosition;
            Debug.Log(tempRotation);


            camera.transform.localEulerAngles = tempRotation;
            camera.transform.localPosition = tempPosition;
            StartCoroutine(AnimateRotation(oldRotation, oldPosition, tempRotation, tempPosition ));
        }


    }

    IEnumerator AnimateRotation(Vector3 fromEul, Vector3 fromPos, Vector3 toEul, Vector3 toPos)
    {

        float speed = 1;

        float elapsedTime = 0;
        while (elapsedTime < .5)
        {
            camera.transform.localEulerAngles = Vector3.Lerp(fromEul, toEul, (elapsedTime / .5f));
            camera.transform.localPosition = Vector3.Lerp(fromPos, toPos, (elapsedTime / .5f));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        camera.transform.localEulerAngles = toEul;
        camera.transform.localPosition = toPos;

        rotating = false;
    }
}
