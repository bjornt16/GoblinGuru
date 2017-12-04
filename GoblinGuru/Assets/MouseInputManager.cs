using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInputManager : MonoBehaviour
{

    private static MouseInputManager instance = null;
    public static MouseInputManager Instance { get { return instance; } }

    public Vector2 current;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void FixedUpdate()
    {
        
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                current = RoundCoordinates(hit.point);
                Debug.Log(current);
                //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(current.x + .5f, 2, current.y + .5f), Quaternion.identity);
            }
        }

    }

    private Vector2 RoundCoordinates(Vector3 coords)
    {
        float roundX = Mathf.Floor(coords.x);
        float roundY = Mathf.Floor(coords.z);

        return new Vector2(roundX, roundY);
    }

}
