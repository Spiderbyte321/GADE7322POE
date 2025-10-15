using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUtilities : MonoBehaviour
{
    public static StateUtilities Instance;

    private Dictionary<EnemyBehaviour, IEnumerator> RunningRoutines = new Dictionary<EnemyBehaviour, IEnumerator>();

    private void Start()
    {
        if(Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }



    public void RunCoroutine(IEnumerator ACoroutineToRun,EnemyBehaviour origin)
    {
        if(origin is null)
            return;
        
        RunningRoutines.TryAdd(origin,ACoroutineToRun);   
        StartCoroutine(ACoroutineToRun);
    }

    public void StopRoutine(EnemyBehaviour origin)
    {
        StopCoroutine(RunningRoutines[origin]);
    }
}
