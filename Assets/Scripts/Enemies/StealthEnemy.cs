using System;
using System.Collections.Generic;
using UnityEngine;

public class StealthEnemy : EnemyBase
{
    private Queue<Tile> Pathorder = new Queue<Tile>();

    private void Update()
    {
        Behaviour.EnemyUpdate();
    }

    protected override void StartEnemy()
    {
        currentHealth = maxHealth;
        
        for (int i = 0; i < PathToFollow.Count; i++)
        {
            Pathorder.Enqueue(PathToFollow[i]);
        }


        Behaviour = new EnemyMovementBehaviour(Pathorder, gameObject);
        Behaviour.EnemyStart();
    }


    public override void TakeDamage(int ADamage, TowerBase ATarget)
    {
        currentHealth--;
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        if(ATarget is not PlayerNexus) 
            return;

        Behaviour = new EnemyAttackingBehaviour(ATarget, AttackDamage, AttackSpeed);
        Behaviour.EnemyStart();
    }

    protected override void TowerDied(TowerBase DeadTower)
    {
        Destroy(gameObject);
    }
}
