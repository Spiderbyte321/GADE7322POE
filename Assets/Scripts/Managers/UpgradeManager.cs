using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField] private Upgrade[] PossibleUpgrades;
    [SerializeField] private string[] UpgradeKeys;
    private Dictionary<string, Upgrade> upgrades = new Dictionary<string, Upgrade>();

    public IReadOnlyDictionary<string, Upgrade> Upgrades => upgrades;

    private void Awake()
    {
        for (int i = 0; i < UpgradeKeys.Length; i++)
        {
            upgrades.TryAdd(UpgradeKeys[i],PossibleUpgrades[i]);
        }
    }
    
}
