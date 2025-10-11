using System;
using System.Collections.Generic;
using UnityEngine;

public class DefenderTower : Tile,Iinteractable
{
    [SerializeField] private ETowerType[] InputTowerTypes;
    [SerializeField] private TowerSpawner[] InputTowerFactories;

    private Dictionary<ETowerType, TowerSpawner> Factories = new Dictionary<ETowerType, TowerSpawner>();
    private TowerSpawner ActiveSpawner = null;


    private void OnEnable()
    {
        PlayerController.OnTowerTypeChosen += SetActiveTower;
    }

    private void OnDisable()
    {
        PlayerController.OnTowerTypeChosen -= SetActiveTower;
    }

    private void Start()
    {
        for (int i = 0; i < InputTowerFactories.Length; i++)
        {
            Factories.Add(InputTowerTypes[i],InputTowerFactories[i]);
        }
        
        SetActiveTower(ETowerType.basic);
    }

    private void SetActiveTower(ETowerType ChosentType)
    {
        ActiveSpawner = Factories[ChosentType];
    }
    

    public void Interact()
    {
        ActiveSpawner.Spawn(gameObject.transform.position);
        Destroy(gameObject);
    }
}
