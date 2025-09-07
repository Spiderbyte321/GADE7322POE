using Unity.Mathematics;
using UnityEngine;

public abstract class TowerSpawner : MonoBehaviour
{
    [SerializeField] private TowerBase TowerToSpawn;


    public virtual void Spawn(Vector3 SpawnPoint)
    {
        Instantiate(TowerToSpawn, SpawnPoint, quaternion.identity);
    }
    
}
