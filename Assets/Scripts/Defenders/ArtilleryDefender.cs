using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryDefender : TowerBase
{

    [SerializeField] private int MaxCharge = 5;
    private List<EnemyBase> Targets = new List<EnemyBase>();
    
    protected override void OnTriggerEnter(Collider other)
    {
        
        base.OnTriggerEnter(other);
        Targets.Add(FoundEnemy);
        
        Debug.Log("colliding with Target");
        if(Targets.Count == 1)
        {
            Debug.Log($"{Targets.Count}");
            StartCoroutine(LaunchShell());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other is null)
            return;
        
        
        if (!other.TryGetComponent(out EnemyBase LeftEnemy))
        {
            return;
        }

        if (Targets.Remove(LeftEnemy))
        {
            Debug.Log("removed Target");
        }
    }


    private IEnumerator LaunchShell()
    {
        int charge = 0;

        while (Targets.Count > 0)
        {
            yield return new WaitForSeconds(AttackSpeed); 
            
            charge+= MaxCharge-AttackSpeed/MaxCharge;
            if (charge < MaxCharge) 
                continue;

            if (Targets[0] is null)
                continue;
            
            Collider[] targetsInRange = Physics.OverlapSphere(Targets[0].transform.position, 1.5f,3);

            foreach (Collider target in targetsInRange)
            {
                target.gameObject.TryGetComponent(out EnemyBase enemyToAttack);
                    
                enemyToAttack.Blast(AttackDamage);
            }

        }
    }
}
