using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyCharacter/Enemy")]
public class CombatEnemy : ScriptableObject {

    public string Name;

    public Color HeadColor;
    public Color TorsoColor;
    public Color LegsColor;
    public Color FeetColor;

    public CharStats Stats;

    public void DoCombatAction()
    {
        Debug.Log("Enemy Does Nothing");
    }

}
