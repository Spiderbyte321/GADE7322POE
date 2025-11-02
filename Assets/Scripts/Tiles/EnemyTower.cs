using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class EnemyTower : Tile
{
    [SerializeField] private int EnemySpawnDelay=2;
    [SerializeField] private int[] AttackSpeedRange;
    [SerializeField] private int[] AttackDamageRange;
    private List<Tile> ConnectedPath = new List<Tile>();

    private void OnEnable()
    {
        GameManager.OnNextWaveStart += StartWave;
    }

    private void OnDisable()
    {
        GameManager.OnNextWaveStart -= StartWave;
    }


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
    
    
    //Rework this to spawn untill the list of enemies is empty rather than forever
    private IEnumerator SpawnEnemy()
    {
        while(WaveManager.instance.CurrentWaveCount>0)
        {
            yield return new WaitForSeconds(EnemySpawnDelay);

            Vector3 SpawnPosition = ConnectedPath[0].transform.position;
            SpawnPosition.y = 1;
            GameObject EnemyToSpawn = WaveManager.instance.TakeEnemyFromWave();
            if (EnemyToSpawn is null)
                continue;
            
            GameObject SpawnedObject =Instantiate(EnemyToSpawn, SpawnPosition,
                quaternion.identity);
            EnemyBase SpawnedEnemy;
            if(!SpawnedObject.TryGetComponent(out SpawnedEnemy))
                throw new Exception("Invalid Spawn should've used factory pattern");

            int SpawnedAttackSpeed = Random.Range(AttackSpeedRange[0], AttackSpeedRange[1]);
            int SpawnedAttackDamage = Random.Range(AttackDamageRange[0], AttackDamageRange[1]);
            SpawnedEnemy.InitialiseEnemy(ConnectedPath,SpawnedAttackSpeed,SpawnedAttackDamage); 
        }
    }
    
    
    private void OnValidate()
    {
        if(AttackSpeedRange.Length != 2)
        {
            Array.Resize(ref AttackSpeedRange,2);
            Debug.Log("Resized AttackSpeedRange to "+2);
        }

        if (AttackDamageRange.Length != 2)
        {
            Array.Resize(ref AttackDamageRange,2);
            Debug.Log("Resized AttackDamageRange to "+2);
        }
        
    }
}
