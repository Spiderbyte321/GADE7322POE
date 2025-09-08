using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour
{
    public abstract void EnemyStart();

    public abstract void EnemyUpdate();
    

    public virtual Queue<Tile> GetRemainingPath()
    {
        throw new NotImplementedException();
    }
    
    protected float RoundToTwoDecimalPLaces(float AFloat)
    {
        float SecondsToWait = AFloat / 60;
        float RoundedSeconds = Mathf.Round(SecondsToWait * 100);
        return RoundedSeconds /=100;
    }
}
