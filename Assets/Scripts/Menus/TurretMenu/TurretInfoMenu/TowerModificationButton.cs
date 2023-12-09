using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class TowerModificationButton : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private TMP_Text m_textObject;
    [SerializeField] private TMP_Text m_priceText;

    [SerializeField] private Sprite m_spriteObject;
    private LevelService m_levelService;
    private InflationService m_inflationService;
    private TowerModificationData m_towerModificationData;
    private ModificationTree m_modificationTree;

    [Inject]
    private void Construct(TowerModificationData towerModificationData,
        ModificationTree modificationTree,
        LevelService levelService,
        InflationService inflationService)
    {
        m_towerModificationData = towerModificationData;
        m_modificationTree = modificationTree;

        m_levelService = levelService;
        m_inflationService = inflationService;
    }

    private void Awake()
    {
        m_modificationTree.AvailableModificationCountChanged += OnAvailableModificationCountChanged;
        m_towerModificationData.UnlockSignalsChanged += SetButtonState;
        m_inflationService.InflationChanged += OnInflationChanged;

        m_button.onClick.AddListener(() =>
        {
            ApplyTowerModification(m_towerModificationData, m_modificationTree.ActiveTowerModule);
        });

        m_textObject.text = m_towerModificationData.Name;
        m_priceText.text = GetInflationCorrectedCost().ToString();

        SetButtonState(!m_towerModificationData.IsBought && m_towerModificationData.UnlockSignals == 0);
    }

    private void OnDestroy()
    {
        m_towerModificationData.UnlockSignalsChanged -= SetButtonState;
        m_modificationTree.AvailableModificationCountChanged -= OnAvailableModificationCountChanged;
        m_inflationService.InflationChanged -= OnInflationChanged;
        m_button.onClick.RemoveAllListeners();
    }

    private void OnInflationChanged() => m_priceText.text = GetInflationCorrectedCost().ToString();

    public void ApplyTowerModification(TowerModificationData towerModificationData, TowerModule towerInfoComponent)
    {
        m_towerModificationData.IsBought = true;
        SetButtonState(false);
        towerModificationData.ApplyModifications(towerInfoComponent);

        m_modificationTree.ApplyTowerModification(towerModificationData);

        m_levelService.Money -= GetInflationCorrectedCost();
    }

    private void OnAvailableModificationCountChanged(int count)
    {
        if (count == 0)
        {
            SetButtonState(false);
        }
    }

    public void SetButtonState(bool isEnabled) => m_button.interactable = isEnabled;

    private int GetInflationCorrectedCost()
    {
        float inflationPercentage = 0;

        foreach (var modification in m_towerModificationData.TowerModifications)
        {
            inflationPercentage += m_inflationService.CalculateInflationPercentage(modification, m_modificationTree.ActiveTowerModule);
        }

        return m_towerModificationData.ModificationCost.AddPercentage(inflationPercentage);
    }

    public class Factory : PlaceholderFactory<TowerModificationData, ModificationTree, TowerModificationButton>
    {
    }
}
