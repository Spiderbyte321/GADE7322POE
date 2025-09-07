using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBase
{
    
    
    private Queue<Tile> PathOrder = new Queue<Tile>();

    private EnemyBehaviour EnemyBehaviour;
    
    protected override void StartEnemy()
    {
        for (int i = 0; i < PathToFollow.Count; i++)
        {
            PathOrder.Enqueue(PathToFollow[i]);
        }
        

        EnemyBehaviour = new EnemyMovementBehaviour(PathOrder,gameObject);
        EnemyBehaviour.EnemyStart();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    { 
        EnemyBehaviour.EnemyUpdate();
    }

    public override void TakeDamage(int ADamage)
    {
        base.TakeDamage(ADamage);

        Queue<Tile> RemainingPath = EnemyBehaviour.EnemyExit();
        PathOrder.Clear();
        int TotalTiles = RemainingPath.Count;

        for (int i = 0; i <= TotalTiles; i++)
        {
            PathOrder.Enqueue(RemainingPath.Dequeue());
        }

        EnemyBehaviour = new EnemyAttackingBehaviour();
        EnemyBehaviour.EnemyStart();
        
    }
}
