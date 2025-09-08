using System;
using System.Collections;
using UnityEngine;

public class StateUtilities : MonoBehaviour
{
    public static StateUtilities Instance;


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



    public void RunCoroutine(IEnumerator ACoroutineToRun)
    {
        StartCoroutine(ACoroutineToRun);
    }

    public void StopRoutine(IEnumerator ACoroutineToStop)
    {
        StopCoroutine(ACoroutineToStop);
    }

    public void StopAllRoutines()
    {
        StopAllCoroutines();
    }
    
    
}
