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



        if (Targets.Count == 0)
        {
            Targets.Enqueue(FoundEnemy);
            StartCoroutine(KillEnemies());
        }
        
        if(Targets.Count < targetMax&&Targets.Count!=0)
        {
            Targets.Enqueue(FoundEnemy);
        }
    }


    private IEnumerator KillEnemies()
    {
        while(Targets.Count>0)
        { 
            CurrentTarget = Targets.Dequeue();
           
            CurrentTarget.TakeDamage(AttackDamage);
            yield return new WaitForSeconds(AttackSpeed);
        }
    }
}
