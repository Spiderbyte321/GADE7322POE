using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] private int BasicTowerCost = 5;

    private ETowerType ChosenType = ETowerType.basic;
    
    private RaycastHit Hit = new RaycastHit();

    private Dictionary<ETowerType, int> TowerCosts = new Dictionary<ETowerType, int>();
    

    private void Start()
    {
        foreach (ETowerType towerType in Enum.GetValues(typeof(ETowerType)))
        {
            switch (towerType)
            {
                case ETowerType.basic:
                    TowerCosts.Add(towerType,BasicTowerCost);
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
        if (GameManager.Instance.PlayerCurrency < TowerCosts[ChosenType]) //change to say not enough
        { 
            return;
        }
           
        
        Iinteractable ClickedObject;
        
        if(Hit.collider.TryGetComponent<Iinteractable>(out ClickedObject)) 
            ClickedObject.Interact();
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
        }


        ChosenType = Tower;
    }
}
