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

    public override void TakeDamage(int ADamage,TowerBase ATarget)
    {
        base.TakeDamage(ADamage,ATarget);

        Queue<Tile> RemainingPath = new Queue<Tile>();
        
        if(EnemyBehaviour is EnemyMovementBehaviour) 
           RemainingPath = EnemyBehaviour.GetRemainingPath();
        
        
        int TotalTiles = RemainingPath.Count;
        
        if(PathOrder.Count!=0)
            PathOrder.Clear();
        
        for(int i = 0; i < TotalTiles; i++)
        {
            PathOrder.Enqueue(RemainingPath.Dequeue());
        }
        

        EnemyBehaviour = new EnemyAttackingBehaviour(Target,AttackDamage,AttackSpeed);
        EnemyBehaviour.EnemyStart();
    }
}
