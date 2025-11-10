using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class BasicDefender : TowerBase//actual tower that will do attacking logic
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(Targets.Count == 1&&FoundEnemy is not null)
        {
            StartCoroutine(KillEnemies());
        }
        
        if(Targets.Count < targetMax&&Targets.Count!=0)
        {
            Targets.Enqueue(FoundEnemy);
        }
    }


    private IEnumerator KillEnemies()
    {
        CurrentTarget = Targets.Dequeue();
        while(CurrentTarget.CurrentHealth>0||Targets.Count>0)
        {
            
            if(CurrentTarget.CurrentHealth <= 0)
                CurrentTarget = Targets.Dequeue();
            
            
            CurrentTarget.TakeDamage(attackDamage,this);

            float RoundedSeconds = RoundToTwoDecimalPLaces(AttackSpeed);
            if(animator.enabled) 
                animator.LoopAnimation();
            
            yield return new WaitForSeconds(RoundedSeconds);
        }  
    }
}
