﻿using System.Collections;
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

    public PlayerUnitUI ui;

    private PathTrail pathTrail;


    int maxStamina;
    int stamina;
    int maxHealth;
    int health;

    public CharStats statistics;

    float shakeIntensity = 0.06f;
    float tempShakeIntensity = 0.06f;


    public bool canCrossRiver;
    public bool canCrossSS;
    public bool canCrossDS;

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
            return (CurrentMovePoints != 0);
        }
    }

    public bool Moving
    {
        get
        {
            return moving;
        }
    }

    public int MaxMovePoints
    {
        get
        {
            return maxMovePoints;
        }
    }

    public int CurrentMovePoints
    {
        get
        {
            return currentMovePoints;
        }
    }

    public PathTrail PathTrail
    {
        get
        {
            return pathTrail;
        }

        set
        {
            pathTrail = value;
        }
    }

    public int CurrentMovePoints1
    {
        get
        {
            return currentMovePoints;
        }
    }

    private void OnEnable()
    {
        GameTurnManager.OnNewTurn += ResetTurn;
    }

    public bool ValidateMove(GameTile dest)
    {
        return true;
        TileTerrain type = dest.tileTerrain;
        if (type == TileTerrain.ShallowSea && canCrossSS)
        {
            return true;
        }
        else if (type == TileTerrain.DeepSea && canCrossDS)
        {
            return true;
        }
        else if (type == TileTerrain.River && canCrossRiver)
        {
            return true;
        }
        else if(type != TileTerrain.ShallowSea && type != TileTerrain.DeepSea && type != TileTerrain.River)
        {
            return true;
        }

        return false;
    }

    void UpdateUI()
    {
        ui.movesText.text = CurrentMovePoints.ToString();
        ui.healthText.text = health + " / " + maxHealth;
        ui.healthSlider.maxValue = maxHealth;
        ui.healthSlider.value = health;
        ui.staminaText.text = stamina + " / " + maxStamina;
        ui.staminaSlider.maxValue = maxStamina;
        ui.staminaSlider.value = stamina;
    }

    void Shake()
    {
        moving = true;
        transform.DetachChildren();
        StartCoroutine(ShakeOverSeconds(.25f));
    }

    private void ResetTurn()
    {
        currentMovePoints = MaxMovePoints;
        //health -= 5;
        //stamina -= 10;
        UpdateUI();
    }

    public void Instantiate(GameTile gameTile)
    {
        statistics = new CharStats();
        tile = gameTile;
        position = tile.Position;
        transform.position = position;

        maxHealth = statistics.maxHP;
        health = statistics.HP;

        maxStamina = statistics.maxStamina;
        stamina = statistics.stamina;

        currentMovePoints = statistics.speed;

        UpdateUI();
    }

    public void GoLeft()
    {
        if (!moving)
        {
            destination = tile.tileLeft;
            Move();
        }
    }
    public void GoRight()
    {
        if (!moving)
        {
            destination = tile.tileRight;
            Move();
        }
    }
    public void GoUp()
    {
        if (!moving)
        {
            destination = tile.tileUp;
            Move();
        }
    }
    public void GoDown()
    {
        if (!moving)
        {
            destination = tile.tileDown;
            Move();
        }
    }

    public void Move()
    {
        if (pathTrail != null && moving == false && CurrentMovePoints > 0 && ValidateMove(destination))
        {
            currentMovePoints -= pathTrail.GetMovementCost();
            UpdateUI();
            moving = true;
            StartCoroutine(TravelPath());
        }
        else if(!moving)
        {
            Shake();
            FindObjectOfType<AudioManager>().Play("errorSound");
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
        position = end;
        transform.position = position;
        tile = destination;
        destination = null;
        moving = false;
        FogOfWar.Instance.ClearFog(tile, 3);
    }

    IEnumerator ShakeOverSeconds(float seconds)
    {
        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.position = position + Random.insideUnitSphere * tempShakeIntensity;
            transform.rotation = new Quaternion(
                Quaternion.identity.x + Random.Range(-tempShakeIntensity, tempShakeIntensity) * .2f,
                Quaternion.identity.y + Random.Range(-tempShakeIntensity, tempShakeIntensity) * .2f,
                Quaternion.identity.z + Random.Range(-tempShakeIntensity, tempShakeIntensity) * .2f,
                Quaternion.identity.w + Random.Range(-tempShakeIntensity, tempShakeIntensity) * .2f);
            tempShakeIntensity -= 0.002f;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        transform.position = position;
        moving = false;
        Camera.main.transform.parent = transform;
        tempShakeIntensity = shakeIntensity;
    }

    protected const float travelSpeed = 4f;

    IEnumerator TravelPath()
    {
        Vector3 a, b, c = pathTrail.PathFromTo[0].Tile.Position;
        transform.localPosition = c;
        //yield return LookAt(pathTrail.PathFromTo[1].Tile.Position);

        float t = Time.deltaTime * travelSpeed;
        bool last = false;
        bool hasBreak = pathTrail.PathBreak.Count != 0;
        int moveCost = 0;

        PathTile pathBreak;
        if (hasBreak)
        {
            pathBreak = pathTrail.PathBreak.First.Value;
            pathTrail.PathBreak.RemoveFirst();
        }
        else
        {
            pathBreak = pathTrail.PathTo;
        }

        pathBreak.Tile.Highlight(true, Color.green);
        int stopIndex = pathTrail.getBreakIndex(pathBreak);

        for (int i = 1; i <= stopIndex; i++)
        {
            if (!last)
            {
                a = c;
                b = pathTrail.PathFromTo[i - 1].Tile.Position;
                c = (b + pathTrail.PathFromTo[i].Tile.Position) * 0.5f;

                moveCost += pathTrail.PathFromTo[i].MoveCost;
            }
            else
            {
                a = c;
                b = (c + pathBreak.Tile.Position) * 0.5f;
                c = pathBreak.Tile.Position;
            }

            if (stopIndex == i && !last)
            {
                i--;
                last = true;
            }

            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                d.y = 0f;
                if (d != Vector3.zero)
                {
                //   transform.localRotation = Quaternion.LookRotation(d);
                }
                yield return null;
            }
            t -= 1f;
        }


        position = pathBreak.Tile.Position;
        transform.position = position;
        tile = pathBreak.Tile;
        destination = null;
        moving = false;
        FogOfWar.Instance.ClearFog(tile, 3);

        pathTrail.ClearUsedTiles(this);
        PathTrail.CalculateTurns(this);

        if(pathTrail.PathFromTo.Count <= 1)
        {
            pathTrail = null;
        }
    }


}
