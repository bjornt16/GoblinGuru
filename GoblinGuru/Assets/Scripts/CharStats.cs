using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharStats {

    public int maxHP;
    public int HP;
    public int maxStamina;
    public int stamina;

    public int level;
    public int currXp;
    public int xpCap;

    public int dexterity;
    public int strength;
    public int wits;
    public int charisma;
    public int speed;

    public int damage;


    // Use this for initialization
    public CharStats()
    {
        maxHP = 20;
        HP = maxHP;
        maxStamina = 20;
        stamina = maxStamina;

        level = 1;
        currXp = 0;
        xpCap = level * 3;
        dexterity = 10;
        strength = 10;
        wits = 10;
        charisma = 10;
        speed = 10;
        speed = 11;
        damage = 10;
    }

    public void ModifyHealth(int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            //todo trigger death
        }
        else if(HP > maxHP)
        {
            HP = maxHP;
        }
    }
}
