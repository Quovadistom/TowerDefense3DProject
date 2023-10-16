using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ModificationTree : MonoBehaviour
{
    [SerializeField] private SelectedTurretMenu m_selectedTurretMenu;
    [SerializeField] private ScrollRect m_scrollRect;
    [SerializeField] private RectTransform m_viewPort;
    [SerializeField] private TMP_Text m_modificationCountText;
    [SerializeField] private int m_availableModificationCount = 5;

    [SerializeField] private Transform m_treeParent;
    [SerializeField] private GameObject m_modificationRow;

    private TowerModule m_activeTurrentInfo;
    private TowerModificationButton.Factory m_towerModificationButtonFactory;

    public event Action<int> AvailableModificationCountChanged;

    public int AvailableModificationCount
    {
        get => m_availableModificationCount;
        set
        {
            m_availableModificationCount = value;
            m_modificationCountText.text = value.ToString();
            AvailableModificationCountChanged?.Invoke(value);
        }
    }

    [Inject]
    private void Construct(TowerModificationButton.Factory towerModificationButtonFactory)
    {
        m_towerModificationButtonFactory = towerModificationButtonFactory;
    }

    void Awake()
    {
        m_selectedTurretMenu.TurretDataChanged += OnTurretChanged;
        AvailableModificationCount = m_availableModificationCount;
    }

    private void OnDestroy()
    {
        m_selectedTurretMenu.TurretDataChanged -= OnTurretChanged;
    }

    private void OnTurretChanged(TowerModule selectedTurret)
    {
        if (selectedTurret != m_activeTurrentInfo)
        {
            foreach (Transform child in m_treeParent)
            {
                Destroy(child.gameObject);
            }
        }

        m_activeTurrentInfo = selectedTurret;

        if (selectedTurret != null)
        {
            CreateTurretModificationMenu(selectedTurret);
        }
    }

    private void CreateTurretModificationMenu(TowerModule selectedTurret)
    {
        TowerModificationTreeData towerModificationTreeData = selectedTurret.ModificationTreeData;

        if (towerModificationTreeData == null)
        {
            return;
        }

        AvailableModificationCount = selectedTurret.AvailableModificationAmount;

        foreach (TowerModificationTreeRow towerModificationTreeStructure in towerModificationTreeData.Structure)
        {
            GameObject row = Instantiate(m_modificationRow, m_treeParent, false);
            foreach (TowerModificationData modification in towerModificationTreeStructure.TowerModifications)
            {
                if (modification.IsBought)
                {
                    AvailableModificationCount--;
                }

                TowerModificationButton button = m_towerModificationButtonFactory.Create();
                button.transform.SetParent(row.transform, false);
                button.SetButtonInfo(towerModificationTreeData, modification, selectedTurret, this);
            }
        }
    }
}
