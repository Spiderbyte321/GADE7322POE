using Unity.Mathematics;
using UnityEngine;

public abstract class TowerSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject TowerToSpawn;


    public virtual void Spawn(Vector3 SpawnPoint)
    {
        Debug.Log("Spawning");
        Instantiate(TowerToSpawn, SpawnPoint, quaternion.identity);
    }
    
}
