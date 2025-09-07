using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class BasicDefender : TowerBase//actual tower that will do attacking logic
{
    [SerializeField]private int maxhealth;
    [SerializeField] private int targetMax;
    [SerializeField] private int AttackSpeed;

    private EnemyBase CurrentTarget;
    private int currenthealth;

    private Queue<EnemyBase> Targets = new Queue<EnemyBase>();

    public int MaxHealth => maxhealth;
    public int CurrentHealth => currenthealth;


    public void TakeDamage(int ADamage)
    {
        currenthealth -= ADamage;
        if(currenthealth<0)
            Destroy(gameObject);
            
    }
    //store an array of enemies and attack them in order
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currenthealth = maxhealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase FoundEnemy = null;
        
        if(!other.TryGetComponent(out FoundEnemy ))
            return;


        if (Targets.Count < targetMax)
        {
            Targets.Enqueue(FoundEnemy);
        }
        else
        {
            //implement enemy walking away requirements
        }

        if(Targets.Count > 0)
        {
            StartCoroutine(AttackEnemy());
        }
        
    }

    private IEnumerator AttackEnemy()
    {
       while(Targets.Count>0)
       { 
           CurrentTarget = Targets.Dequeue();
           
           //attack current target
         yield return new WaitForSeconds(AttackSpeed);
       }
    }
}
