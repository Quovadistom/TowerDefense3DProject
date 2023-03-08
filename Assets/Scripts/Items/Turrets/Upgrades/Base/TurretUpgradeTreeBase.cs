using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class TurretUpgradeTreeBase : MonoBehaviour
{
    [SerializeField] private TowerUpgradeComponent m_upgradeComponent;
    [SerializeField] private UILineRenderer m_lineRendererAsset;
    [SerializeField] private Transform m_lineParent;

    public bool IsUpgradeLeft => m_upgradeComponent.TowerAvailableUpgradeCount >= 0;
    public event Action AllUpgradesSpend;

    private void Awake()
    {
        m_upgradeComponent.AvailableUpgradeCountChanged += OnAvailableUpgradeCountChanged;
    }

    private void OnAvailableUpgradeCountChanged(int count)
    {
        if (count == 0)
        {
            AllUpgradesSpend?.Invoke();
        }
    }

    public void LowerUpgradeCount()
    {
        m_upgradeComponent.TowerAvailableUpgradeCount--;
    }

    public List<string> GetUnlockedUpgrades()
    {
        List<string> unlockedUpgrades = new List<string>();

        UpgradeNode[] nodes = GetComponentsInChildren<UpgradeNode>(true);

        foreach (UpgradeNode node in nodes.Where(node => node.IsBought))
        {
            if (node.TryGetComponent(out IIDProvider idProvider))
            {
                unlockedUpgrades.Add(idProvider.ID);
            }
        }

        return unlockedUpgrades;
    }

    public void SetUnlockedUpgrades(List<string> unlockedUpgrades)
    {
        UpgradeNode[] nodes = GetComponentsInChildren<UpgradeNode>(true);

        foreach (UpgradeNode node in nodes)
        {
            if (node.TryGetComponent(out IIDProvider idProvider) && unlockedUpgrades.Contains(idProvider.ID))
            {
                node.OnButtonClick();
            }
        }
    }


    [Button]
    private void UpdateAllLineRenderers()
    {
        foreach (Transform child in m_lineParent)
        {
            DestroyImmediate(child.gameObject);
        }

        UpgradeNode[] nodes = GetComponentsInChildren<UpgradeNode>(true);
        foreach (UpgradeNode node in nodes)
        {
            foreach (UpgradeNode unlockNode in node.UnlockedByNodes)
            {
                Vector2[] coordinates = new Vector2[]
                {
                    m_lineParent.InverseTransformPoint(node.transform.position),
                    m_lineParent.InverseTransformPoint(unlockNode.transform.position)
                };

                UILineRenderer lineRenderer = Instantiate(m_lineRendererAsset, m_lineParent);
                lineRenderer.Points = coordinates;
            }
        }
    }
}
