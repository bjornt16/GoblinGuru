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

    private PlayerUnit player;

    public void DoCombatAction()
    {
        if(player == null)
        {
            player = GameStateManager.Instance.player;
        }

        Debug.Log("Enemy Does Nothing");
    }

}
