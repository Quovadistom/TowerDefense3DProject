using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AvailableTileModificationButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private TMP_Text m_modificationAmountText;
    [SerializeField] private Button m_button;
    [SerializeField] private GameObject m_modificationAppliedCheck;

    private ModificationContainer m_connectedModification;
    private int m_modificationAmount;
    private TownHousingService m_townHousingService;
    private Guid m_id;
    private int m_connectedLocation;

    [Inject]
    private void Construct(Guid id, TownHousingService townHousingService)
    {
        m_id = id;
        m_townHousingService = townHousingService;
    }

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
        m_modificationAppliedCheck.SetActive(housingData.ActiveModifications[location] == m_connectedModification);
    }

    public void SetButtonInfo(ModificationContainer connectedModification, int modificationAmount, int connectedLocation)
    {
        m_connectedModification = connectedModification;
        m_modificationAmount = modificationAmount;
        m_text.text = m_connectedModification.Name;
        m_connectedLocation = connectedLocation;

        m_button.onClick.AddListener(OnButtonClicked);

        m_modificationAmountText.text = modificationAmount.ToString();

        m_modificationAppliedCheck.SetActive(m_townHousingService.GetHousingData(m_id).ActiveModifications[m_connectedLocation] == m_connectedModification);
    }

    private void OnButtonClicked()
    {
        m_townHousingService.ModificateTile(m_id, m_connectedModification, m_connectedLocation);
    }

    public class Factory : PlaceholderFactory<Guid, AvailableTileModificationButton> { }
}
