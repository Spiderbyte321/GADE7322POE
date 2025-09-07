using UnityEngine;
using System.Collections.Generic;
public abstract class EnemyBase : MonoBehaviour
{
    protected int maxHealth = 150;
    protected int currentHealth;
    protected EnemyBehaviour Behaviour;

    public int CurrentHealth => currentHealth;
    
    protected List<Tile> PathToFollow = new List<Tile>();
    public void InitialiseEnemy(List<Tile> APathToFollow)
    {
        foreach (Tile tile in APathToFollow)
        {
            PathToFollow.Add(tile);
        }
        StartEnemy();
    }

    protected abstract void StartEnemy();

    public virtual void TakeDamage(int ADamage)
    {
        currentHealth -= ADamage;
    }
}
