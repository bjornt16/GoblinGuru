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
    [SerializeField]
    private int currentMovePoints;

    public GameTile Tile
    {
        get
        {
            return tile;
        }
    }

    public bool CanMove
    {
        get
        {
            return (currentMovePoints != 0);
        }
    }

    public bool Moving
    {
        get
        {
            return moving;
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
        transform.position = position;
    }

    public void goLeft()
    {
        if (tile.tileUp != null && !moving && currentMovePoints != 0)
        {
            destination = tile.tileLeft;
            Move();
        }
    }
    public void goRight()
    {
        if (tile.tileUp != null && !moving && currentMovePoints != 0)
        {
            destination = tile.tileRight;
            Move();
        }
    }
    public void goUp()
    {
        if (tile.tileUp != null && !moving && currentMovePoints != 0)
        {
            destination = tile.tileUp;
            Move();
        }
    }
    public void goDown()
    {
        if (tile.tileUp != null && !moving && currentMovePoints != 0)
        {
            destination = tile.tileDown;
            Move();
        }
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
