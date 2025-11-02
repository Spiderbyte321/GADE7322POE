using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] private int BasicTowerCost = 5;
    [SerializeField] private int SpeedTowerCost = 6;
    [SerializeField] private int ArtilerryTowerCost = 4;
    [SerializeField] private UpgradeManager UpgradeManager;

    private ETowerType ChosenType = ETowerType.basic;
    
    private RaycastHit Hit = new RaycastHit();

    private Dictionary<ETowerType, int> TowerCosts = new Dictionary<ETowerType, int>();

    private bool isUpgrading = false;

    private string chosenUpgrade;

    public delegate void ChoseTowerTypeAction(ETowerType chosentower);

    public static event ChoseTowerTypeAction OnTowerTypeChosen;
    

    private void Start()
    {
        foreach (ETowerType towerType in Enum.GetValues(typeof(ETowerType)))
        {
            switch (towerType)
            {
                case ETowerType.basic:
                    TowerCosts.Add(towerType,BasicTowerCost);
                    break;
                case ETowerType.speed:
                    TowerCosts.Add(towerType,SpeedTowerCost);
                    break;
                case ETowerType.artilery:
                    TowerCosts.Add(towerType,ArtilerryTowerCost);
                    break;
            }
        }
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(!Physics.Raycast(ray, out Hit)) 
                return;

            if(isUpgrading)
            {
                HandleMouseUpgrade();
            }
            else
            {
               HandleMouseInteract(); 
            }
            
        }
    }

    private void HandleMouseInteract()
    {
        
        if(GameManager.Instance.PlayerCurrency < TowerCosts[ChosenType]) //change to say not enough
        { 
            return;
        }
           
       
        
        Iinteractable ClickedObject;

        if (!Hit.collider.TryGetComponent(out ClickedObject)) 
            return;
        
        ClickedObject.Interact();
        GameManager.Instance.DecreaseCurrency(TowerCosts[ChosenType]);

    }

    private void HandleMouseUpgrade()
    {
        Debug.Log("Player Chose to upgrade");
        //keeps a dictionary of ints and strings 
        //no can't cause then we're not using the towers upgrades
        //ugggggh
        //hmm Iupgradable coul return the upgrade manager
        //uggggh I should've planned this better but I have to keep moving on

        IUpgradable upgradable;
        if (!Hit.collider.TryGetComponent(out upgradable))
        {
            Debug.Log(Hit.collider.name);
           return; 
        }
        
        
        if(GameManager.Instance.PlayerCurrency < UpgradeManager.Upgrades[chosenUpgrade].UpgradeCost)
        {
            Debug.Log("Poor");
            return;
        }
        
        upgradable.Upgrade(UpgradeManager.Upgrades[chosenUpgrade]);
        GameManager.Instance.DecreaseCurrency(UpgradeManager.Upgrades[chosenUpgrade].UpgradeCost);
        isUpgrading = false;
    }

    public void SetChosenType(string AChosentType)
    {
        AChosentType = AChosentType.ToLower();
        ETowerType Tower = ETowerType.basic;

        switch (AChosentType)
        {
            case "basic":
                Tower = ETowerType.basic;
                break;
            case "speed":
                Tower = ETowerType.speed;
                break;
            case "artilery":
                Tower = ETowerType.artilery;
                break;
        }


        ChosenType = Tower;
        
        OnTowerTypeChosen?.Invoke(ChosenType);
    }

    public void SetChosenUpgrade(string AChosenUpgrade)
    {
        //Don't like this but time is of the essence so I will keep it like this and move on
        chosenUpgrade = AChosenUpgrade;
        isUpgrading = true;
    }
}
