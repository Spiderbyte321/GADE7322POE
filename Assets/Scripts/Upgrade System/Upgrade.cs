using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    [SerializeField] private int maxHealthIncrease;
    [SerializeField] private int attackDamageIncrease;
    [SerializeField] private int attackSpeedIncrease;
    [SerializeField] private int currentHealthIncrease;
    [SerializeField] private int upgradeCost;

    public int MaxHealthIncrease => maxHealthIncrease;
    public int AttackDamageIncrease => attackDamageIncrease;
    public int AttackSpeedIncrease => attackSpeedIncrease;
    public int CurrentHealthIncrease => currentHealthIncrease;

    public int UpgradeCost => upgradeCost;

}
