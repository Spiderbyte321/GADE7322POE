using TMPro;
using UnityEngine;

public class AllyHealthCanvasController : HealthCanvasControllers
{
    [SerializeField] private TextMeshProUGUI maxhealthText;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI attackDamageText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TowerBase attachedTower;
    
    
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
    
    private void UpdateText()
   {
       Debug.Log("Updating text");
       maxhealthText.text = attachedTower.MaxHealth+"";
       currentHealthText.text = attachedTower.CurrentHealth + "";
       attackDamageText.text = attachedTower.AttackDamage + "";
       attackSpeedText.text = attachedTower.AttackSpeed + "";
   }
}
