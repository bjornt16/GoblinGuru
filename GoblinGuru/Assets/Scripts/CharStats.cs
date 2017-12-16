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
    public int armorBonus;
    public int minDamage;
    public int maxDamage;
    public int bonusDamage;

    // Use this for initialization
    public CharStats()
    {
        maxHP = 20;
        HP = maxHP;
        maxStamina = 20;
        stamina = maxStamina;

        level = 1;
        currXp = 0;
        xpCap = level * 4;
        dexterity = 5;
        strength = 5;
        wits = 5;
        charisma = 5;
        speed = 6;
        armorBonus = 0;
        minDamage = 1;
        maxDamage = 8;
        bonusDamage = 0;
    }

    public int GetArmor()
    {
        return 10 + dexterity + armorBonus;
    }

    public int GetDexModifier()
    {
        return (dexterity - 5) / 2;
    }

    public int GetStrModifier()
    {
        return (strength - 5) / 2;
    }

    public int GetWitsModifier()
    {
        return (wits - 5) / 2;
    }

    public int GetChaModifier()
    {
        return (charisma - 5) / 2;
    }

    public int DexRoll()
    {
        int randomNum = (int)Random.RandomRange(1f, 21f);

        return randomNum + GetDexModifier();
    }

    public int StrRoll()
    {
        int randomNum = (int)Random.RandomRange(1f, 21f);

        return randomNum + GetStrModifier();
    }

    public int WitsRoll()
    {
        int randomNum = (int)Random.RandomRange(1f, 21f);

        return randomNum + GetWitsModifier();
    }

    public int ChaRoll()
    {
        int randomNum = (int)Random.RandomRange(1f, 21f);

        return randomNum + GetChaModifier();
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
        xpCap = level * 4;
    }

    System.Random rnd = new System.Random(123);

    public bool Roll(CharacterStats roll, int dc)
    {
        int d20 = rnd.Next(1, 21);
        Debug.Log("d20 roll " + d20 + " dc is " + dc);
        switch (roll)
        {
            case CharacterStats.Dexterity:
                return (d20 + dexterity) >= dc;
            case CharacterStats.Strength:
                return (d20 + dexterity) >= dc;
            case CharacterStats.Wits:
                return (d20 + dexterity) >= dc;
            case CharacterStats.Charisma:
                return (d20 + dexterity) >= dc;
            case CharacterStats.Speed:
                return (d20 + dexterity) >= dc;
            default:
                return d20 >= dc;
        }
    }

    public bool Roll(CharacterStats roll, CharStats targetStats)
    {
        int d20 = rnd.Next(1, 21);
        int tD20 = rnd.Next(1, 21);
        switch (roll)
        {
            case CharacterStats.Dexterity:
                return (d20 + dexterity) >= (tD20 + targetStats.dexterity);
            case CharacterStats.Strength:
                return (d20 + strength) >= (tD20 + targetStats.strength);
            case CharacterStats.Wits:
                return (d20 + wits) >= (tD20 + targetStats.wits);
            case CharacterStats.Charisma:
                return (d20 + charisma) >= (tD20 + targetStats.charisma);
            case CharacterStats.Speed:
                return (d20 + speed) >= (tD20 + targetStats.speed);
            default:
                return d20 >= tD20;
        }
    }
    
}



public enum CharacterStats
{
    D20, Dexterity, Strength, Wits, Charisma, Speed
}

public enum CharacterStatsCheck
{
    Hp, Dexterity, Strength, Wits, Charisma, Speed, Stamina
}

public enum SleepType
{
    none, terrible, bad, normal, good, great
}