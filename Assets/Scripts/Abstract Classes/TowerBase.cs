using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public abstract class TowerBase : MonoBehaviour,IUpgradable
{
    
    [SerializeField] protected int maxhealth;
    [SerializeField] protected int targetMax;
    [SerializeField] protected float attackSpeed; 
    [SerializeField] protected int attackDamage;
    [SerializeField] protected HealthBarController HealthBar;
    [SerializeField] private GameObject unupgradedMesh;
    [SerializeField] private GameObject upgradedMesh;
    
    

    protected EnemyBase CurrentTarget;
    protected int currenthealth;
    protected EnemyBase FoundEnemy;

   
    
    protected Queue<EnemyBase> Targets = new Queue<EnemyBase>();
    
    public delegate void TowerDiedAction(TowerBase deadTower);

    public static event TowerDiedAction OnTowerDied;

    public delegate void TowerStatsModifiedAction();

    public static event TowerStatsModifiedAction OnTowerStatsModified;
    

    public int MaxHealth => maxhealth;
    public int CurrentHealth => currenthealth;

    public float AttackSpeed => attackSpeed;

    public int AttackDamage => attackDamage;

    public virtual void TakeDamage(int ADamage)
    {
        currenthealth -= ADamage;
        HealthBar.SetHealth(currenthealth);
        if (currenthealth < 0)
        {
            TowerDied(); 
            Destroy(gameObject);
           TerrainGenerator.Instance.ReplaceSpot(gameObject.transform);
        }
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
        attackDamage += AUpgrade.AttackDamageIncrease;
        attackSpeed += AUpgrade.AttackSpeedIncrease;
        if (unupgradedMesh is not null)
        {
           unupgradedMesh.SetActive(false); 
        }
        upgradedMesh.SetActive(true);
        OnTowerStatsModified?.Invoke();
    }

    protected float RoundToTwoDecimalPLaces(float AFloat)
    {
        float SecondsToWait = AFloat/ 60;
        float RoundedSeconds = Mathf.Round(SecondsToWait * 100);
       return RoundedSeconds /=100;
    }

    private void TowerDied()
    {
         OnTowerDied?.Invoke(this);
    }

    public void Upgrade(Upgrade AUpgrade)
    {
        ApplyUpgrade(AUpgrade);
    }
    
    
}
