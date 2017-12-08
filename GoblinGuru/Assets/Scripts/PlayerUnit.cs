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


    int maxStamina;
    int stamina;
    int maxHealth;
    int health;

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

    private void OnEnable()
    {
        GameTurnManager.OnNewTurn += ResetTurn;
    }

    public bool ValidateMove(GameTile dest)
    {
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
        ui.movesText.text = currentMovePoints.ToString();
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
        currentMovePoints = maxMovePoints;
        health -= 5;
        stamina -= 10;
        UpdateUI();
    }

    public void Instantiate(GameTile gameTile)
    {
        tile = gameTile;
        position = tile.Position;
        transform.position = position;

        maxHealth = 20;
        health = 20;

        maxStamina = 20;
        stamina = 20;

        currentMovePoints = maxMovePoints;

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
        if(destination != null && moving == false && currentMovePoints > 0 && ValidateMove(destination))
        {
            currentMovePoints--;
            UpdateUI();
            moving = true;
            StartCoroutine(MoveOverSeconds(destination.Position, travelSpeedinSeconds));
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


}
