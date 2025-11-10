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
        if(FoundEnemy is not null)
        {
            Targets.Add(FoundEnemy);
        }
        
        
        if(Targets.Count == 1)
        {
            StartCoroutine(LaunchShell());
        }
    }
    

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
                animator.LoopAnimation();
            }

            charge = 0;
        }
    }
}
