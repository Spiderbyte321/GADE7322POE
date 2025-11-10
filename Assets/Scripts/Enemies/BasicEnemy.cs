using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBase
{
    private Queue<Tile> PathOrder = new Queue<Tile>();
    
    protected override void StartEnemy()
    {
        for (int i = 0; i < PathToFollow.Count; i++)
        {
            PathOrder.Enqueue(PathToFollow[i]);
        }


        Behaviour = movementBehaviour;
        Behaviour.EnemyStart();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        Behaviour.EnemyUpdate();
    }

    public override void TakeDamage(int ADamage,TowerBase ATarget)
    {
        base.TakeDamage(ADamage,ATarget);
        
        Behaviour = attackingBehaviour;
        Behaviour.Retarget(Target);
        Behaviour.EnemyStart();
    }


    protected override void TowerDied(TowerBase DeadTower)
    {
        if(DeadTower is PlayerNexus)
            return;
        
        if(Target == DeadTower)
        {
            Behaviour = movementBehaviour;
            Behaviour.EnemyStart();
        }
    }
}
