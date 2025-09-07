using UnityEngine;
using System.Collections.Generic;

public abstract class TowerBase : MonoBehaviour
{
    
    [SerializeField]protected int maxhealth;
    [SerializeField] protected int targetMax;
    [SerializeField] protected int AttackSpeed;
    [SerializeField] protected int AttackDamage;

    protected EnemyBase CurrentTarget;
    protected int currenthealth;
    protected EnemyBase FoundEnemy;

    protected Queue<EnemyBase> Targets = new Queue<EnemyBase>();

    public int MaxHealth => maxhealth;
    public int CurrentHealth => currenthealth;


    public virtual void TakeDamage(int ADamage)
    {
        currenthealth -= ADamage;
        if(currenthealth<0)
            Destroy(gameObject);
            
    }
    
    
    void Start()
    {
        currenthealth = maxhealth;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered parent");
        if(!other.TryGetComponent(out FoundEnemy ))
            return;


        if (Targets.Count < targetMax)
        {
            Targets.Enqueue(FoundEnemy);
        }
       
        
    }

    protected float RoundToTwoDecimalPLaces(float AFloat)
    {
        float SecondsToWait = AFloat/ 60;
        float RoundedSeconds = Mathf.Round(SecondsToWait * 100);
       return RoundedSeconds /=100;
    }
}
