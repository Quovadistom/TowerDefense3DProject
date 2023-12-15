using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlacementItemButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_titleText;
    [SerializeField] private Button m_button;
    private TownTile m_tile;
    private TowerAssets m_towerAssets;
    private MenuController m_menuController;
    private ResourceService m_resourceService;

    [Inject]
    private void Construct(ResourceService resourceService)
    {
        m_resourceService = resourceService;
    }

    private void Awake()
    {
        m_resourceService.ResourceChanged += OnResourceChanged;
        m_button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        m_resourceService.ResourceChanged -= OnResourceChanged;
        m_button.onClick.RemoveAllListeners();
    }

    private void OnResourceChanged(object sender, ResourcesChangeEventArgs e)
    {
        if (e.Resource is Scraps)
        {
            SetButtonState();
        }
    }

    private void SetButtonState()
    {
        m_button.interactable = m_resourceService.HasAllResources(m_towerAssets.HousingPrefab.RequiredResources);
    }

    public void InitializeButton(TownTile tile, TowerAssets towerAssets, MenuController menuController)
    {
        m_tile = tile;
        m_towerAssets = towerAssets;
        m_menuController = menuController;
        SetButtonState();
    }

    private void OnButtonClick()
    {
        m_tile.UpdateTile(m_towerAssets);
        m_menuController.PopMenuPage();
        m_resourceService.RemoveAvailableResources(m_towerAssets.HousingPrefab.RequiredResources);
    }

    internal class Factory : PlaceholderFactory<PlacementItemButton> { }
}
