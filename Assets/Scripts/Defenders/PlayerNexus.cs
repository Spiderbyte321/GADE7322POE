using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerNexus : TowerBase//attacking logic for player tower
{
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out FoundEnemy))
        {
             return;
        }
           
        
        
        if(Targets.Count > 0)
        {
          Targets.Enqueue(FoundEnemy);  
        }
        else
        {
            Targets.Enqueue(FoundEnemy);
            StartCoroutine(KillEnemies());
        }
        
    }
    


    private IEnumerator KillEnemies()
    {
        CurrentTarget = Targets.Dequeue();
        while(CurrentTarget.CurrentHealth>0||Targets.Count>0)
        {
            
            if(CurrentTarget.CurrentHealth <= 0)
                CurrentTarget = Targets.Dequeue();
            
            
            CurrentTarget.TakeDamage(attackDamage,this);

            float RoundedSeconds = RoundToTwoDecimalPLaces(attackSpeed);
            
             yield return new WaitForSeconds(RoundedSeconds);
        }  
    }
}
