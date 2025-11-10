using TMPro;
using UnityEngine;

public class AllyHealthCanvasController : HealthCanvasControllers
{
    [SerializeField] protected TextMeshProUGUI maxhealthText;
    [SerializeField] protected TextMeshProUGUI attackDamageText;
    [SerializeField] protected TextMeshProUGUI attackSpeedText;
    [SerializeField] protected TowerBase attachedTower;
    
    
    private void OnEnable()
    {
        TowerBase.OnTowerStatsModified += UpdateText;
    }
    
    private void OnDisable()
    {
        TowerBase.OnTowerStatsModified -= UpdateText;
    }
    
    private void Start()
    {
        UpdateText();
    }
    
    protected virtual void UpdateText()
   {
       maxhealthText.text = attachedTower.MaxHealth+"";
       attackDamageText.text = attachedTower.AttackDamage + "";
       attackSpeedText.text = attachedTower.AttackSpeed + "";
   }
}
