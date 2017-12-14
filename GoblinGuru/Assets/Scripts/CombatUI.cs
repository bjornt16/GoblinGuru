using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Diagnostics;

public class CombatUI : MonoBehaviour {

    public UnityEngine.UI.RawImage Ground;
    public GameObject[] GroundMelee;
    public GameObject[] GroundRanged;
    public GameObject[] GroundDistant;

    public GameObject CardPanel;
    public GameObject CardParent;

    public UnityEngine.UI.Slider playerHealth;
    public UnityEngine.UI.Slider playerStamina;

    public UnityEngine.UI.Slider targetHealth;
    public UnityEngine.UI.Slider targetStamina;
    public TextMeshProUGUI combatRoundText;
    public TextMeshProUGUI combatStatusText;

    public TextMeshProUGUI playerTurnText;
    public TextMeshProUGUI targetTurnText;

    public void UpdateUI(CharStats player, CharStats target)
    {
        playerHealth.maxValue = player.maxHP;
        playerHealth.value = player.HP;
        playerStamina.maxValue = player.maxStamina;
        playerStamina.value = player.stamina;

        targetHealth.maxValue = target.maxHP;
        targetHealth.value = target.HP;
        targetStamina.maxValue = target.maxStamina;
        targetStamina.value = target.stamina;
    }

    public void SetRangeMelee(CombatViewModel player, CombatViewModel target)
    {
        player.transform.parent = GroundMelee[0].transform;
        target.transform.parent = GroundMelee[1].transform;
        ResetTransformPosition(player, target);
    }

    public void SetRangeRanged(CombatViewModel player, CombatViewModel target)
    {
        player.transform.parent = GroundRanged[0].transform;
        target.transform.parent = GroundRanged[1].transform;
        ResetTransformPosition(player, target);
    }

    public void SetRangeDistant(CombatViewModel player, CombatViewModel target)
    {
        player.transform.parent = GroundDistant[0].transform;
        target.transform.parent = GroundDistant[1].transform;
        ResetTransformPosition(player, target);
    }

    private void ResetTransformPosition(CombatViewModel player, CombatViewModel target)
    {
        player.transform.localPosition = new Vector3(0, 0, 0);
        target.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void Sleep(int ms)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        while (true)
        {
            //some other processing to do STILL POSSIBLE
            if (stopwatch.ElapsedMilliseconds >= ms)
            {
                break;
            }
            System.Threading.Thread.Sleep(1); //so processor can rest for a while
        }
    }


    public void CombatStatusHit()
    {
        combatStatusText.text = "Hit!";
        combatStatusText.color = Color.red;
    }

    public void CombatStatusMiss()
    {
        combatStatusText.text = "Miss!";
        combatStatusText.color = Color.gray;
    }

    public void SetPlayerTurn()
    {
        targetTurnText.enabled = false;
        playerTurnText.enabled = true;
    }

    public void SetTargetTurn()
    {
        playerTurnText.enabled = false;
        targetTurnText.enabled = true;
    }

    internal void CombatEnd(CharStats player, CharStats target)
    {
        if(player.HP <= 0)
        {
            //todo game over.
        }
        else
        {
            //combat won
        }
    }
}
