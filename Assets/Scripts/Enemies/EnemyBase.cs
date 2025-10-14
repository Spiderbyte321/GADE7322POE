using System;
using UnityEngine;
using System.Collections.Generic;
public abstract class EnemyBase : MonoBehaviour
{
    
    [SerializeField]protected int maxHealth = 150;
    [SerializeField]protected int AttackDamage;
    [SerializeField]protected int AttackSpeed;
    [SerializeField] protected HealthBarController healthbar;
    protected TowerBase Target;
    protected int currentHealth;
    protected EnemyBehaviour Behaviour;

    public int CurrentHealth => currentHealth;
    
    protected List<Tile> PathToFollow = new List<Tile>();


    private void OnEnable()
    {
        TowerBase.OnTowerDied += TowerDied;
    }

    private void OnDisable()
    {
        TowerBase.OnTowerDied -= TowerDied;
    }


    public void InitialiseEnemy(List<Tile> APathToFollow,int AAttackSpeed,int AAttackDamage)
    {
        foreach (Tile tile in APathToFollow)
        {
            PathToFollow.Add(tile);
        }

        AttackSpeed += AAttackSpeed;
        AttackDamage += AAttackDamage;

        if (AttackSpeed <= 0)//Safety Nets
        {
            AttackSpeed = 10;
            
        }

        if (AttackDamage <= 0)
        {
            AttackDamage = 1;
        }
        
        healthbar.InitialiseHealthBar(maxHealth);
        StartEnemy();
    }

    protected abstract void StartEnemy();

    public virtual void TakeDamage(int ADamage,TowerBase ATarget)
    {
        currentHealth -= ADamage;
        healthbar.SetHealth(currentHealth);

        if(currentHealth<=0)
        {
          Destroy(gameObject);  
        }

        Target = ATarget;
    }

    public void Blast(int ADamage)
    {
        currentHealth -= ADamage;
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        
        Debug.Log("Blasted");
    }




    protected abstract void TowerDied(TowerBase DeadTower);
}
