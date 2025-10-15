using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] private int BasicTowerCost = 5;
    [SerializeField] private int SpeedTowerCost = 6;
    [SerializeField] private int ArtilerryTowerCost = 4;

    private ETowerType ChosenType = ETowerType.basic;
    
    private RaycastHit Hit = new RaycastHit();

    private Dictionary<ETowerType, int> TowerCosts = new Dictionary<ETowerType, int>();

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

            if(Physics.Raycast(ray, out Hit)) 
                HandleMouseClick();
        }
    }

    private void HandleMouseClick()
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
}
