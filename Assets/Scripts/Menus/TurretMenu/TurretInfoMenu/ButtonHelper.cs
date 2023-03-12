using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonHelper : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private TMP_Text m_textObject;
    [SerializeField] private Sprite m_spriteObject;

    public TowerUpgradeTreeData TowerUpgradeTree { get; private set; }
    public TowerUpgradeData TowerUpgradeData { get; private set; }
    public IEnumerable<TowerUpgradeData> LockedTowerUpgrades => TowerUpgradeTree.GetTowerUpgradesDatas(TowerUpgradeData.RequiredFor);

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
        TowerUpgradeData.UnlockSignalsChanged -= SetButtonState;
    }

    public void SetButtonInfo(TowerUpgradeTreeData towerUpgradeTreeStructure, TowerUpgradeData towerUpgradeData, TowerInfoComponent towerInfoComponent)
    {
        m_textObject.text = towerUpgradeData.m_id;
        TowerUpgradeTree = towerUpgradeTreeStructure;
        TowerUpgradeData = towerUpgradeData;

        bool isButtonActive = !TowerUpgradeData.IsBought && TowerUpgradeData.UnlockSignals == 0;
        TowerUpgradeData.UnlockSignalsChanged += SetButtonState;
        SetButtonState(isButtonActive);

        m_button.onClick.AddListener(() =>
        {
            ApplyTowerUpgrade(towerUpgradeData, towerInfoComponent);
        });
    }

    public void ApplyTowerUpgrade(TowerUpgradeData towerUpgradeData, TowerInfoComponent towerInfoComponent)
    {
        TowerUpgradeData.IsBought = true;
        SetButtonState(false);
        towerUpgradeData.TowerUpgrade.TryApplyUpdate(towerInfoComponent);
        foreach (var towerData in LockedTowerUpgrades)
        {
            towerData.UnlockSignals--;
        }
    }

    public void SetButtonState(bool isEnabled)
    {
        m_button.interactable = isEnabled;
    }
}
