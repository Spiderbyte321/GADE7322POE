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

        if(Targets.Count == 1)
        {
            StartCoroutine(LaunchShell());
        }
    }


    private IEnumerator LaunchShell()
    {
        int charge = 0;

        while (Targets.Count > 0)
        {
            yield return new WaitForSeconds(1); 
            
            charge++;

            if (charge < MaxCharge) 
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
