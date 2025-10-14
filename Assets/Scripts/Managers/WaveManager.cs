using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{

    [SerializeField] private List<float> difficultyThresholds;
    [SerializeField] private List<GameObject> EnemyObjects;

    private Queue<GameObject> EnemiesToGive = new Queue<GameObject>();
    private List<float> normalisedThresholds = new List<float>();
    private Dictionary<float, GameObject> EnemyThresholds = new Dictionary<float, GameObject>();
    private List<GameObject> waveToSpawn = new List<GameObject>();
    private float graphOffset;
    
    private int waveCount = 0;

    private float DifficultyThreshold;
    private float waveTotalThreshold=0;

    public int CurrentWaveCount => waveToSpawn.Count;

    public static WaveManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    void Start()
    {
        waveCount++;
        DifficultyThreshold = 3 * waveCount;

        float maxThreshold=0;
        foreach (float threshold in difficultyThresholds)//get our max value
        {
            if (threshold > maxThreshold)
            {
                maxThreshold = threshold;
            }
        }

        foreach (float threshold in difficultyThresholds)//Normalize our thresholds
        {
            normalisedThresholds.Add(threshold/maxThreshold);
        }

        for(int i=0;i<normalisedThresholds.Count;i++)//add them to  the dictionary
        {
            EnemyThresholds.Add(normalisedThresholds[i],EnemyObjects[i]);
        }

        graphOffset = 0.5f;
        
        CreateWave();
    }

    private void CreateWave()
    {
        while (waveTotalThreshold < DifficultyThreshold)
        {

            float GivenThreshold = Mathf.PerlinNoise1D(graphOffset);

            graphOffset += GivenThreshold;

            float randomiser = Random.Range(GivenThreshold - 2 * GivenThreshold, GivenThreshold);

            GivenThreshold += randomiser / GivenThreshold;

            float chosenThreshold = normalisedThresholds[0];
            for (int i = 0; i < normalisedThresholds.Count; i++)
            {
                if (normalisedThresholds[i] < GivenThreshold && normalisedThresholds[i + 1] > GivenThreshold)
                {
                    chosenThreshold = normalisedThresholds[i];
                }
            }

            waveToSpawn.Add(EnemyThresholds[chosenThreshold]);
            waveTotalThreshold += chosenThreshold;


            foreach (GameObject enemy in waveToSpawn)
            {
                EnemiesToGive.Enqueue(enemy);
            }
        }
    }

    public GameObject TakeEnemyFromWave()
    {
        return EnemiesToGive.Count == 0 ? null : EnemiesToGive.Dequeue();
    }
}
