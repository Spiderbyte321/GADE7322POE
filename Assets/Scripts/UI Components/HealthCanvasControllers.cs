using System;
using TMPro;
using UnityEngine;

public class HealthCanvasControllers : MonoBehaviour
{
    private Camera mainCamera;


    private void OnEnable()
    {
        //TowerBase.OnTowerUpgraded += UpdateText;
    }
    
    private void OnDisable()
    {
        //TowerBase.OnTowerUpgraded -= UpdateText;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        //UpdateText();
    }

    void Update()
    {
        gameObject.transform.LookAt(mainCamera.transform);
    }

    /*private void UpdateText()
    {
        maxhealthText.text = attachedTower.MaxHealth+"";
        currentHealthText.text = attachedTower.CurrentHealth + "";
        attackDamageText.text = attachedTower.AttackDamage + "";
        attackSpeedText.text = attachedTower.AttackSpeed + "";
    }*/
}
