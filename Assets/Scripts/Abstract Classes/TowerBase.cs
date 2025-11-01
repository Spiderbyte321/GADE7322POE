using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class TowerBase : MonoBehaviour,IUpgradable
{
    
    [SerializeField] protected int maxhealth;
    [SerializeField] protected int targetMax;
    [SerializeField] protected float AttackSpeed;
    [SerializeField] protected int AttackDamage;
    [SerializeField] protected HealthBarController HealthBar;
    [SerializeField] private GameObject unupgradedMesh;
    [SerializeField] private GameObject upgradedMesh;
    
    

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

    private void ApplyUpgrade(Upgrade AUpgrade)
    {
        maxhealth += AUpgrade.MaxHealthIncrease;
        currenthealth += AUpgrade.CurrentHealthIncrease;
        AttackDamage += AUpgrade.AttackDamageIncrease;
        AttackSpeed += AUpgrade.AttackSpeedIncrease;
        unupgradedMesh.SetActive(false);
        upgradedMesh.SetActive(true);
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

    public void Upgrade(Upgrade AUpgrade)
    {
        ApplyUpgrade(AUpgrade);
    }
    
}
