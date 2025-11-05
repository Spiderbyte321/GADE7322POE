using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryDefender : TowerBase
{

    [SerializeField] private int MaxCharge = 5;
    [SerializeField] private HealthBarController ChargeBar;
    private List<EnemyBase> Targets = new List<EnemyBase>();


    private void OnEnable()
    {
        EnemyBase.OnEnemyDied += ReactToDeadEnemy;
    }

    private void OnDisable()
    {
        EnemyBase.OnEnemyDied -= ReactToDeadEnemy;
    }

    private void ReactToDeadEnemy(EnemyBase deadEnemy)
    {
        if(Targets.Contains(deadEnemy)) 
            Targets.Remove(deadEnemy);
    }

    protected override void Start()
    {
        base.Start();
        ChargeBar.InitialiseHealthBar(MaxCharge);
        ChargeBar.SetHealth(0);
    }
    

    protected override void OnTriggerEnter(Collider other)
    {
       
        
        base.OnTriggerEnter(other);
         Debug.Log("collided");
        if(FoundEnemy is not null)
        {
            Debug.Log("Adding");
            Targets.Add(FoundEnemy);
        }
        
        
        if(Targets.Count == 1)
        {
            //Debug.Log($"{Targets.Count}");
            StartCoroutine(LaunchShell());
        }
    }

    /*private void OnTriggerExit(Collider other)
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
    }*/
    
    //ok no there needs to be a better way
    // how about instead we get the transfrom and use the position rather than getting it from the enemy


    private IEnumerator LaunchShell()
    {
        int charge = 0;
        EnemyBase positiontarget = null;
        while (Targets.Count > 0)
        {
            yield return new WaitForSeconds(attackSpeed);

            charge++;
            ChargeBar.SetHealth(charge);
            if (charge < MaxCharge) 
                continue;
            

            if(Targets.Count == 0)
            {
              continue;  
            }

            foreach (EnemyBase target in Targets)
            {
                if (target is not null)
                    positiontarget = target;
            }
            

            Transform TargetPosition = positiontarget.transform;
            
            Collider[] targetsInRange = Physics.OverlapSphere(TargetPosition.position, 1.5f);
            
            if(targetsInRange.Length==0)
                continue;
            
            foreach (Collider collider in targetsInRange)
            {
                EnemyBase enemyTarget;

                if(collider is null)
                {
                    continue;
                }
                
                if(!collider.gameObject.TryGetComponent(out enemyTarget))
                {
                    continue;
                }
                
                enemyTarget.Blast(attackDamage);
                
            }

        }
    }
}
