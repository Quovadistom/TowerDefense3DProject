using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class TowerModificationButton : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private TMP_Text m_textObject;
    [SerializeField] private Sprite m_spriteObject;
    private LevelService m_levelService;
    private DifficultyService m_difficultyService;

    public TowerModificationTreeData TowerModificationTree { get; private set; }
    public TowerModificationData TowerModificationData { get; private set; }
    public ModificationTree ModificationTree { get; private set; }

    public IEnumerable<TowerModificationData> LockedTowerModifications => TowerModificationTree.GetTowerModificationsDatas(TowerModificationData.RequiredFor.Select(id => Guid.Parse(id)));

    [Inject]
    private void Construct(LevelService levelService, DifficultyService difficultyService)
    {
        m_levelService = levelService;
        m_difficultyService = difficultyService;
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
        TowerModificationData.UnlockSignalsChanged -= SetButtonState;
        ModificationTree.AvailableModificationCountChanged -= OnAvailableModificationCountChanged;
    }

    public void SetButtonInfo(TowerModificationTreeData towerModificationTreeStructure,
        TowerModificationData towerModificationData,
        TowerModule towerInfoComponent,
        ModificationTree modificationTree)
    {
        m_textObject.text = towerModificationData.Name;
        TowerModificationTree = towerModificationTreeStructure;
        TowerModificationData = towerModificationData;
        ModificationTree = modificationTree;
        ModificationTree.AvailableModificationCountChanged += OnAvailableModificationCountChanged;

        bool isButtonActive = !TowerModificationData.IsBought && TowerModificationData.UnlockSignals == 0;
        TowerModificationData.UnlockSignalsChanged += SetButtonState;
        SetButtonState(isButtonActive);

        m_button.onClick.AddListener(() =>
        {
            ApplyTowerModification(towerModificationData, towerInfoComponent);
        });
    }

    public void ApplyTowerModification(TowerModificationData towerModificationData, TowerModule towerInfoComponent)
    {
        TowerModificationData.IsBought = true;
        SetButtonState(false);
        towerModificationData.ApplyModifications(towerInfoComponent);
        foreach (var towerData in LockedTowerModifications)
        {
            towerData.UnlockSignals--;
        }
        ModificationTree.AvailableModificationCount--;
        m_levelService.Money -= TowerModificationData.ModificationCost;
    }

    private void OnAvailableModificationCountChanged(int count)
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

    public class Factory : PlaceholderFactory<TowerModificationButton>
    {
    }
}
