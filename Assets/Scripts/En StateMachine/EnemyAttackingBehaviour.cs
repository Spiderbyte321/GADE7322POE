using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingBehaviour : EnemyBehaviour//attacks back at what's attacking
{
    private int AttackSpeed;
    private int AttackDamage;
    private TowerBase Target;
    public override void EnemyStart()
    {
        StateUtilities.Instance.StartCoroutine(KillTower());
    }

    public override void EnemyUpdate()
    {
        
    }

    public override void EnemyExit()
    {
    }

    public EnemyAttackingBehaviour(TowerBase ATarget,int AAttackDamage,int AAttackSpeed)
    {
        Target = ATarget;
        AttackDamage = AAttackDamage;
        AttackSpeed = AAttackSpeed;
    }


    private IEnumerator KillTower()
    { 
        Debug.Log("trying to kill a tower");
        while(true)
        {
          Target.TakeDamage(AttackDamage);
          float TimeToWait = RoundToTwoDecimalPLaces(AttackSpeed / 60);
          yield return new WaitForSeconds(TimeToWait);
        }
    }
}
