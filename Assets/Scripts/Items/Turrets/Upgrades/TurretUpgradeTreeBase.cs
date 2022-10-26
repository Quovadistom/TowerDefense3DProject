using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class TurretUpgradeTreeBase : MonoBehaviour
{
    [SerializeField] UILineRenderer m_lineRendererAsset;
    [SerializeField] Transform m_lineParent;

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
