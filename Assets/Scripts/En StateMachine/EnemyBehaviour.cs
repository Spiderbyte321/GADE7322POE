using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour
{
    public abstract void EnemyStart();

    public abstract void EnemyUpdate();

    public virtual Queue<Tile> EnemyExit()
    {
        throw new Exception("Trying to exit wrong object");
    }
}
