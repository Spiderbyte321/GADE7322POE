using UnityEngine;
using System.Collections.Generic;
public abstract class EnemyBase : MonoBehaviour
{
    protected TowerBase Target;
    protected int maxHealth = 150;
    protected int currentHealth;
    protected EnemyBehaviour Behaviour;
    protected int AttackDamage;
    protected int AttackSpeed;

    public int CurrentHealth => currentHealth;
    
    protected List<Tile> PathToFollow = new List<Tile>();
    public void InitialiseEnemy(List<Tile> APathToFollow,int AAttackSpeed,int AAttackDamage)
    {
        foreach (Tile tile in APathToFollow)
        {
            PathToFollow.Add(tile);
        }

        AttackSpeed = AAttackSpeed;
        AttackDamage = AAttackDamage;
        StartEnemy();
    }

    protected abstract void StartEnemy();

    public virtual void TakeDamage(int ADamage,TowerBase ATarget)
    {
        currentHealth -= ADamage;

        if(currentHealth<=0)
        {
          Destroy(gameObject);  
        }

        Target = ATarget;
    }
}
