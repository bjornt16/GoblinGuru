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
        damage = 10;
    }

    public void ModifyHealth(int damage)
    {
        HP += damage;
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

    public void UpdateXpCap()
    {
        xpCap = level * 3;
    }

    System.Random rnd = new System.Random(123);

    public bool Roll(CharacterStats roll, int dc)
    {
        int d20 = rnd.Next(1, 21);
        switch (roll)
        {
            case CharacterStats.Dexterity:
                return (d20 + dexterity) > dc;
            case CharacterStats.Strength:
                return (d20 + dexterity) > dc;
            case CharacterStats.Wits:
                return (d20 + dexterity) > dc;
            case CharacterStats.Charisma:
                return (d20 + dexterity) > dc;
            case CharacterStats.Speed:
                return (d20 + dexterity) > dc;
            default:
                return d20 > dc;
        }
    }

    public bool Roll(CharacterStats roll, CharStats targetStats)
    {
        int d20 = rnd.Next(1, 21);
        int tD20 = rnd.Next(1, 21);
        switch (roll)
        {
            case CharacterStats.Dexterity:
                return (d20 + dexterity) > (tD20 + targetStats.dexterity);
            case CharacterStats.Strength:
                return (d20 + strength) > (tD20 + targetStats.strength);
            case CharacterStats.Wits:
                return (d20 + wits) > (tD20 + targetStats.wits);
            case CharacterStats.Charisma:
                return (d20 + charisma) > (tD20 + targetStats.charisma);
            case CharacterStats.Speed:
                return (d20 + speed) > (tD20 + targetStats.speed);
            default:
                return d20 > tD20;
        }
    }
}

public enum CharacterStats
{
    D20, Dexterity, Strength, Wits, Charisma, Speed
}