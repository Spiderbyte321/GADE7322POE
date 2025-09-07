using System.Collections;
using UnityEngine;

public class PlayerNexus : TowerBase//attacking logic for player tower
{
    protected override void OnTriggerEnter(Collider other)
    {

        if(!other.TryGetComponent(out FoundEnemy))
            return;
        
        Targets.Enqueue(FoundEnemy);

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
        while (Targets.Count>0)
        {
            yield return new WaitForSeconds(AttackSpeed);
            CurrentTarget.TakeDamage(AttackDamage);
        }  
    }
}
