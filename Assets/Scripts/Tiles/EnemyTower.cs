using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class EnemyTower : Tile
{
    [SerializeField] private int EnemySpawnDelay=2;
    [SerializeField] private GameObject[] EnemyTypes;
    private List<Tile> ConnectedPath = new List<Tile>();
    
    
    void Start()
    {
        foreach (Tile tile in TerrainGenerator.Instance.EnemyPaths[this])
        {
            ConnectedPath.Add(tile);
        }

        ConnectedPath.Reverse();
        ConnectedPath.RemoveAt(0);
        StartWave();
    }

    private void StartWave()//use an event to trigger this
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(EnemySpawnDelay);

            Vector3 SpawnPosition = ConnectedPath[0].transform.position;
            SpawnPosition.y = 1;
            GameObject SpawnedObject =Instantiate(EnemyTypes[Random.Range(0, EnemyTypes.Length)], SpawnPosition,
                quaternion.identity);
            EnemyBase SpawnedEnemy;
            if(SpawnedObject.TryGetComponent(out SpawnedEnemy))
                SpawnedEnemy.InitialiseEnemy(ConnectedPath);

        }
    }
}
