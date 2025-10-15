using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class TowerBase : MonoBehaviour
{
    
    [SerializeField]protected int maxhealth;
    [SerializeField] protected int targetMax;
    [SerializeField] protected int AttackSpeed;
    [SerializeField] protected int AttackDamage;
    [SerializeField] protected HealthBarController HealthBar;
    
    

    protected EnemyBase CurrentTarget;
    protected int currenthealth;
    protected EnemyBase FoundEnemy;

   
    
    protected Queue<EnemyBase> Targets = new Queue<EnemyBase>();
    
    public delegate void TowerDiedAction(TowerBase deadTower);

    public static event TowerDiedAction OnTowerDied;

    public int MaxHealth => maxhealth;
    public int CurrentHealth => currenthealth;
    

    public virtual void TakeDamage(int ADamage)
    {
        currenthealth -= ADamage;
        HealthBar.SetHealth(currenthealth);
        if(currenthealth<0)
            Destroy(gameObject);
            
    }
    
    
    protected virtual void Start()
    {
        currenthealth = maxhealth;
        HealthBar.InitialiseHealthBar(currenthealth);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
        if(other is null)
            return;
        
        
        if (!other.TryGetComponent(out FoundEnemy))
        {
            return;
        }
        

        if(Targets.Count < targetMax)
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

    private void OnDestroy()
    {
        OnTowerDied?.Invoke(this);
    }
}
