using System;
using System.Collections.Generic;
using UnityEngine;

public class StealthEnemy : EnemyBase
{
    private Queue<Tile> Pathorder = new Queue<Tile>();
    private EnemyBehaviour behaviour;

    private void Update()
    {
        behaviour.EnemyUpdate();
    }

    protected override void StartEnemy()
    {
        currentHealth = maxHealth;
        
        for (int i = 0; i < PathToFollow.Count; i++)
        {
            Pathorder.Enqueue(PathToFollow[i]);
        }


        behaviour = new EnemyMovementBehaviour(Pathorder, gameObject);
        behaviour.EnemyStart();
    }


    public override void TakeDamage(int ADamage, TowerBase ATarget)
    {
        Debug.Log("Attacking");
        currentHealth--;
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        if(ATarget is not PlayerNexus) 
            return;

        Debug.Log("attacking");
        behaviour = new EnemyAttackingBehaviour(ATarget, AttackDamage, AttackSpeed);
        behaviour.EnemyStart();
    }

    protected override void TowerDied(TowerBase DeadTower)
    {
        Destroy(gameObject);
    }
}
