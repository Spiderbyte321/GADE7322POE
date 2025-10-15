using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryDefender : TowerBase
{

    [SerializeField] private int MaxCharge = 5;
    [SerializeField] private HealthBarController ChargeBar;
    private List<EnemyBase> Targets = new List<EnemyBase>();

    protected override void Start()
    {
        base.Start();
        ChargeBar.InitialiseHealthBar(MaxCharge);
        ChargeBar.SetHealth(0);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
        base.OnTriggerEnter(other);

        if(FoundEnemy is not null)
        {
            Debug.Log("Adding");
            Targets.Add(FoundEnemy);
        }
        
        
        //Debug.Log("colliding with Target");
        if(Targets.Count == 1)
        {
            //Debug.Log($"{Targets.Count}");
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
        int currentTarget = 0;

        while (Targets.Count > 0)
        {
            yield return new WaitForSeconds(AttackSpeed);

            charge++;
            ChargeBar.SetHealth(charge);
            if (charge < MaxCharge) 
                continue;
            
            if (Targets.Count==0)
            {
                Debug.Log("No enemies to attack");
                StopCoroutine(LaunchShell());
            }
            
            {
                if (Targets[currentTarget] is null)
                {
                    currentTarget += 1;
                }
            }
                
            Debug.Log("getting targets");
            Collider[] targetsInRange = Physics.OverlapSphere(Targets[currentTarget].transform.position, 1.5f,3);

            foreach (Collider target in targetsInRange)
            {
                Debug.Log("Launching");
                target.gameObject.TryGetComponent(out EnemyBase enemyToAttack);
                    
                enemyToAttack.Blast(AttackDamage);
            }

        }
    }
}
