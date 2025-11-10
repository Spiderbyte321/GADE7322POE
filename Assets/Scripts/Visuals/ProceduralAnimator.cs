using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ProceduralAnimator : MonoBehaviour
{

    [SerializeField] private Transform[] PositionsToMoveTo;
    [SerializeField] private Transform[] ExplosionOrigins;
    [SerializeField] private GameObject mainTarget;
    [SerializeField] private float AnimationSpeed=1; 
    [SerializeField] private Rigidbody mainBody;
    private Vector3 mainOrigin;
    private int AnimationIndex =0;
    private float T = 0;
    private bool isRunning;

    private void Awake()
    {
        mainOrigin = mainBody.position;
    }


    public void LoopAnimation()
    {
        if(! gameObject.activeSelf)
            return;
        
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        while(AnimationIndex != PositionsToMoveTo.Length)
        {
            Vector3 TargetVector = PositionsToMoveTo[AnimationIndex].position;
            Vector3 MoveVector = Vector3.Lerp(mainTarget.transform.position, TargetVector, T);
                    
            T += AnimationSpeed * Time.deltaTime;
                    
            mainTarget.transform.position = MoveVector;
                    
            if(mainTarget.transform.position == TargetVector)
            {
                T = 0;
                AnimationIndex++;
            }
            yield return null;
        }
    }

    public void PlayDamagedAnimation()
    {
        if(!gameObject.activeSelf)
            return;
        
        PlayRandomised();
    }

    void PlayRandomised()
    {
        if(!isRunning)
            StartCoroutine(randomAnimationEnumerator());
    }

    private IEnumerator randomAnimationEnumerator()//used for main bone movement
    {
        isRunning = true;
        mainBody.isKinematic = false;
         mainBody.AddExplosionForce(1000,ExplosionOrigins[Random.Range(0,
                 ExplosionOrigins.Length)].position,
             5);
         yield return new WaitForSeconds(0.1f);
         mainBody.isKinematic = true;
         float t = 0;
         while (mainTarget.transform.position !=mainOrigin)
         {
             yield return null;
             Vector3 MoveVector = Vector3.Lerp(mainTarget.transform.position, mainOrigin, t);
        
             t += AnimationSpeed * Time.deltaTime;
             
             mainTarget.transform.position = MoveVector;
         }
         isRunning = false;
    }
    
}
