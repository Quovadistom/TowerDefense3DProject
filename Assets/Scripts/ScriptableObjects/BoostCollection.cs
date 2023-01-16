using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesCollection", menuName = "ScriptableObjects/UpgradesCollection")]
public class BoostCollection : ScriptableObject
{
    [SerializeField] private List<TowerBoostBase> m_upgradesList;

    public IReadOnlyList<TowerBoostBase> UpgradesList { get => m_upgradesList; }
}
