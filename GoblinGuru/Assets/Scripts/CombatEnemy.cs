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

    private Combat combat;

    public void DoCombatAction()
    {
        if (Stats.isPlayer)
        {
            Stats.isPlayer = false;
        }
        if(player == null)
        {
            player = GameStateManager.Instance.player;
        }
        if(combat == null)
        {
            combat = Combat.Instance;
        }


        int dmg = 0 - (UnityEngine.Random.Range(Stats.minDamage, Stats.maxDamage) + Stats.bonusDamage);
        if (player.target != null)
        {
            player.statistics.ModifyHealth(dmg);
            Debug.Log("target damages player for " + dmg + " damage");
            combat.CombatUI.CombatStatusHit();
        }

    }

}
