using System;
using System.Collections.Generic;
using System.Linq;
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

    public event Action<int> AvailableModificationCountChanged;

    public TowerModule ActiveTowerModule { get; private set; }

    private TowerModificationButton.Factory m_towerModificationButtonFactory;
    private List<TowerModificationButton> m_towerButtons = new();

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

    public TowerModificationTreeData TowerModificationTreeData { get; private set; }

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
        if (selectedTurret != ActiveTowerModule)
        {
            foreach (Transform child in m_treeParent)
            {
                Destroy(child.gameObject);
            }
        }

        ActiveTowerModule = selectedTurret;

        if (selectedTurret != null)
        {
            CreateTurretModificationMenu(selectedTurret);
        }
    }

    private void CreateTurretModificationMenu(TowerModule selectedTurret)
    {
        m_towerButtons.Clear();

        TowerModificationTreeData = selectedTurret.ModificationTreeData;

        if (TowerModificationTreeData == null)
        {
            return;
        }

        AvailableModificationCount = selectedTurret.AvailableModificationAmount;

        foreach (TowerModificationTreeRow towerModificationTreeStructure in TowerModificationTreeData.Structure)
        {
            GameObject row = Instantiate(m_modificationRow, m_treeParent, false);
            foreach (TowerModificationData modification in towerModificationTreeStructure.TowerModifications)
            {
                if (modification.IsBought)
                {
                    AvailableModificationCount--;
                }

                TowerModificationButton button = m_towerModificationButtonFactory.Create(modification, this);
                button.transform.SetParent(row.transform, false);

                m_towerButtons.Add(button);
            }
        }
    }

    public void ApplyTowerModification(TowerModificationData towerModificationData)
    {
        foreach (var towerData in TowerModificationTreeData.GetTowerModificationsDatas(towerModificationData.RequiredFor.Select(id => Guid.Parse(id))))
        {
            towerData.UnlockSignals--;
        }

        AvailableModificationCount--;
    }
}
