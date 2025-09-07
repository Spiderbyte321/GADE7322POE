using UnityEngine;

public class DefenderTower : Tile,Iinteractable
{
    [SerializeField] private GameObject DefenderPrefab;
    
    
    //spawn a tower prefab
    //that prefab has the collision sphere
    //That prefab will have a script on it

    public void Interact()
    {
        
    }
}
