using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingBehaviour : EnemyBehaviour//attacks back at what's attacking
{
    public override void EnemyStart()
    {
        throw new System.NotImplementedException();
    }

    public override void EnemyUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override Queue<Tile> EnemyExit()
    {
        throw new Exception("Not supposed to exit this");
    }

    public EnemyAttackingBehaviour()
    {
        
    }
}
