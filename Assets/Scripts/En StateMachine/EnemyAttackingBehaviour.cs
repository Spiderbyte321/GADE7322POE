using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingBehaviour : EnemyBehaviour//attacks back at what's attacking
{
    private int AttackSpeed;
    private int AttackDamage;
    private TowerBase Target;
    private bool routineFinished;
    public override void EnemyStart()
    {
        if(AttackDamage==0||AttackSpeed==0)
            return;
        
        StartCoroutine(KillTower());
    }

    public override void Retarget(TowerBase ATarget)
    {
        Target = ATarget;
    }

    public override void EnemyUpdate()
    {
        
    }

    public override void EnemyEnd()
    {
        StopCoroutine(KillTower());
    }


    public void InitialiseEnemyAttacking(int AAttackDamage,int AAttackSpeed)
    {
        AttackDamage = AAttackDamage;
        AttackSpeed = AAttackSpeed;
    }


    private IEnumerator KillTower()
    { 
        while(true)
        {
            if(Target == null)
            { 
                break;
            }
                
            
            Target.TakeDamage(AttackDamage); 
            //float TimeToWait = RoundToTwoDecimalPLaces(AttackSpeed / 60); 
            yield return new WaitForSeconds(AttackSpeed);
            
        }
    }
}
