using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyMovementBehaviour : EnemyBehaviour
{

    private GameObject EnemyObject;
    private Queue<Tile> PathToFollow = new Queue<Tile>();
    private Tile TargetTile;
    private float Movespeed = 0.5f;

    private float T = 0;
    
    public override void EnemyStart()
    {
        if(PathToFollow.Count>0) 
            TargetTile = PathToFollow.Dequeue();
    }

    public override void EnemyUpdate()
    {
        if(TargetTile == null)
            return;
        
        Vector3 TargetVector = new Vector3(TargetTile.transform.position.x, 1, TargetTile.transform.position.z);
        
        Vector3 MoveVector = Vector3.Lerp(EnemyObject.transform.position, TargetVector, T);
        
         T += Movespeed * Time.deltaTime;
        
        EnemyObject.transform.position = MoveVector;
        
        if(PathToFollow.Count==1)
            return;
        
        if(EnemyObject.transform.position == TargetVector)
        {
            T = 0;
            TargetTile = PathToFollow.Dequeue();
        }
    }

    

    public override Queue<Tile> GetRemainingPath()
    {
        return PathToFollow;
    }


    public EnemyMovementBehaviour(Queue<Tile> APathToFollow,GameObject AEnemyObject)
    {
        int OriginalLength = APathToFollow.Count;
        
        for(int i = 0;i<OriginalLength;i++)
        {
            PathToFollow.Enqueue(APathToFollow.Dequeue());
        }

        if(AEnemyObject is not null)
        {
           EnemyObject = AEnemyObject; 
        }
            
    }
}
