using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour {
    [SerializeField]
    GameTile tile;
    Vector3 position;

    bool moving = false;

    public float travelSpeedinSeconds;
    protected const float rotationSpeed = 180f;

    protected float orientation;

    public GameTile destination;

    [SerializeField]
    private int maxMovePoints = 3;
    private int currentMovePoints;

    public GameTile Tile
    {
        get
        {
            return tile;
        }
    }

    private void Start()
    {
        currentMovePoints = maxMovePoints;
    }

    private void OnEnable()
    {
        GameTurnManager.OnNewTurn += ResetTurn;
    }

    private void ResetTurn()
    {
        currentMovePoints = maxMovePoints;
    }

    public void Instantiate(GameTile gameTile)
    {
        tile = gameTile;
        position = tile.Position;
    }

    public void Move()
    {
        if(destination != null && moving == false && currentMovePoints > 0)
        {
            currentMovePoints--;
            moving = true;
            StartCoroutine(MoveOverSeconds(destination.Position, travelSpeedinSeconds));
        }
    }

    IEnumerator LookAt(Vector3 point)
    {
        point.y = transform.localPosition.y;
        Quaternion fromRotation = transform.localRotation;
        Quaternion toRotation = Quaternion.LookRotation(point - transform.localPosition);
        float angle = Quaternion.Angle(fromRotation, toRotation);
        if (angle > 0f)
        {
            float speed = rotationSpeed / angle;

            for (float t = Time.deltaTime * speed; t < 1f; t += Time.deltaTime * speed)
            {
                transform.localRotation = Quaternion.Slerp(fromRotation, toRotation, t);
                yield return null;
            }

            transform.LookAt(point);
            orientation = transform.localRotation.eulerAngles.y;
        }
    }

    IEnumerator MoveOverSpeed(Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        while (transform.position != end)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator MoveOverSeconds(Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
        tile = destination;
        destination = null;
        moving = false;
    }


}
