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
    private InflationService m_inflationService;
    private ResourceService m_resourceService;
    private TowerModificationData m_towerModificationData;
    private ModificationTree m_modificationTree;

    [Inject]
    private void Construct(TowerModificationData towerModificationData,
        ModificationTree modificationTree,
        InflationService inflationService,
        ResourceService resourceService)
    {
        m_towerModificationData = towerModificationData;
        m_modificationTree = modificationTree;
        m_inflationService = inflationService;
        m_resourceService = resourceService;
    }

    private void Awake()
    {
        m_modificationTree.AvailableModificationCountChanged += OnAvailableModificationCountChanged;
        m_towerModificationData.UnlockSignalsChanged += OnUnlockSignalsChanged;
        m_inflationService.InflationChanged += OnInflationChanged;
        m_resourceService.ResourceChanged += OnResourceChanged;

        m_button.onClick.AddListener(() =>
        {
            ApplyTowerModification(m_towerModificationData, m_modificationTree.ActiveTowerModule);
        });

        m_textObject.text = m_towerModificationData.Name;
        m_priceText.text = GetInflationCorrectedCost().ToString();

        SetButtonState();
    }

    private void OnDestroy()
    {
        m_modificationTree.AvailableModificationCountChanged -= OnAvailableModificationCountChanged;
        m_towerModificationData.UnlockSignalsChanged -= OnUnlockSignalsChanged;
        m_inflationService.InflationChanged -= OnInflationChanged;
        m_resourceService.ResourceChanged -= OnResourceChanged;
        m_button.onClick.RemoveAllListeners();
    }

    private void OnUnlockSignalsChanged(bool obj) => SetButtonState();

    private void OnInflationChanged()
    {
        m_priceText.text = GetInflationCorrectedCost().ToString();
        SetButtonState();
    }

    private void OnResourceChanged(object sender, ResourcesChangeEventArgs e)
    {
        if (e.Resource is BattleFunds)
        {
            SetButtonState();
        }
    }

    public void ApplyTowerModification(TowerModificationData towerModificationData, TowerModule towerInfoComponent)
    {
        m_towerModificationData.IsBought = true;
        SetButtonState();
        towerModificationData.ApplyModifications(towerInfoComponent);

        m_modificationTree.ApplyTowerModification(towerModificationData);

        m_resourceService.ChangeAvailableResource<BattleFunds>(-GetInflationCorrectedCost());
    }

    private void OnAvailableModificationCountChanged(int count) => SetButtonState();

    public void SetButtonState()
    {
        bool canBuy = m_modificationTree.AvailableModificationCount > 0 && !m_towerModificationData.IsBought;
        bool isUnlocked = m_towerModificationData.UnlockSignals == 0;
        bool hasResources = m_resourceService.HasResource<BattleFunds>(GetInflationCorrectedCost());

        m_button.interactable = canBuy && isUnlocked && hasResources;
    }

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
