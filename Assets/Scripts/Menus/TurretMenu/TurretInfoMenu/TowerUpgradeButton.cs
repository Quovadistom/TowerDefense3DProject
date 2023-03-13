using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class TowerUpgradeButton : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private TMP_Text m_textObject;
    [SerializeField] private Sprite m_spriteObject;
    private LevelService m_levelService;
    private DifficultyService m_difficultyService;

    public TowerUpgradeTreeData TowerUpgradeTree { get; private set; }
    public TowerUpgradeData TowerUpgradeData { get; private set; }
    public UpgradeTree UpgradeTree { get; private set; }

    public IEnumerable<TowerUpgradeData> LockedTowerUpgrades => TowerUpgradeTree.GetTowerUpgradesDatas(TowerUpgradeData.RequiredFor);

    [Inject]
    private void Construct(LevelService levelService, DifficultyService difficultyService)
    {
        m_levelService = levelService;
        m_difficultyService = difficultyService;
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
        TowerUpgradeData.UnlockSignalsChanged -= SetButtonState;
        UpgradeTree.AvailableUpgradeCountChanged -= OnAvailableUpgradeCountChanged;
    }

    public void SetButtonInfo(TowerUpgradeTreeData towerUpgradeTreeStructure,
        TowerUpgradeData towerUpgradeData,
        TowerInfoComponent towerInfoComponent,
        UpgradeTree upgradeTree)
    {
        m_textObject.text = towerUpgradeData.m_id;
        TowerUpgradeTree = towerUpgradeTreeStructure;
        TowerUpgradeData = towerUpgradeData;
        UpgradeTree = upgradeTree;
        UpgradeTree.AvailableUpgradeCountChanged += OnAvailableUpgradeCountChanged;

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
        UpgradeTree.AvailableUpgradeCount--;
        m_levelService.Money -= TowerUpgradeData.UpgradeCost;
    }

    private void OnAvailableUpgradeCountChanged(int count)
    {
        if (count == 0)
        {
            SetButtonState(false);
        }
    }

    public void SetButtonState(bool isEnabled)
    {
        m_button.interactable = isEnabled;
    }

    public class Factory : PlaceholderFactory<TowerUpgradeButton>
    {
    }
}
