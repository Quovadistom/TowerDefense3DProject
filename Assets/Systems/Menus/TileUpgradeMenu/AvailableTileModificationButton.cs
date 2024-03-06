using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AvailableTileModificationButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private Button m_button;
    [SerializeField] private GameObject m_modificationAppliedCheck;

    private Blueprint m_connectedBluePrint;
    private TownHousingService m_townHousingService;
    private BlueprintService m_blueprintService;
    private Guid m_id;
    private int m_connectedLocation;

    [Inject]
    private void Construct(Guid id, TownHousingService townHousingService, BlueprintService blueprintService)
    {
        m_id = id;
        m_townHousingService = townHousingService;
        m_blueprintService = blueprintService;
    }

    //TODO: Enable when already bought, so one can easily sell 
    private void Awake()
    {
        m_townHousingService.TileModificationApplied += OnTileModificationApplied;
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
        m_townHousingService.TileModificationApplied -= OnTileModificationApplied;
    }

    private void OnTileModificationApplied(HousingData housingData, int location)
    {
        m_modificationAppliedCheck.SetActive(housingData.ActiveModifications[location] == m_connectedBluePrint);
    }

    public void SetButtonInfo(Blueprint blueprint, int connectedLocation)
    {
        m_connectedBluePrint = blueprint;
        m_text.text = m_connectedBluePrint.Name;
        m_connectedLocation = connectedLocation;

        m_button.onClick.AddListener(OnButtonClicked);

        bool isInstalled = m_townHousingService.GetHousingData(m_id).ActiveModifications[m_connectedLocation] == m_connectedBluePrint;
        m_modificationAppliedCheck.SetActive(isInstalled);

        if (isInstalled)
        {
            transform.SetAsFirstSibling();
        }

        m_button.interactable = m_blueprintService.CanBuyBlueprint(m_connectedBluePrint);
    }

    private void OnButtonClicked()
    {
        m_townHousingService.ModificateTile(m_id, m_connectedBluePrint, m_connectedLocation);
    }

    public class Factory : PlaceholderFactory<Guid, AvailableTileModificationButton> { }
}
