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
        

        Behaviour = new EnemyMovementBehaviour(PathOrder,gameObject);
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

        Queue<Tile> RemainingPath = new Queue<Tile>();

        if(Behaviour is EnemyMovementBehaviour)
             RemainingPath = Behaviour.GetRemainingPath();
        
          
        
        
        int TotalTiles = RemainingPath.Count;
        
        if(PathOrder.Count!=0)
            PathOrder.Clear();
        
        for(int i = 0; i < TotalTiles; i++)
        {
            PathOrder.Enqueue(RemainingPath.Dequeue());
        }
        

        Behaviour = new EnemyAttackingBehaviour(Target,AttackDamage,AttackSpeed);
        Behaviour.EnemyStart();
    }


    protected override void TowerDied(TowerBase DeadTower)
    {
        if(DeadTower is PlayerNexus)
            return;
        if(Target == DeadTower)
        {
            Behaviour = new EnemyMovementBehaviour(PathOrder, gameObject);
            Behaviour.EnemyStart();
        }
    }
}
