using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradeTreeBase : MonoBehaviour
{
    [Button]
    private void UpdateAllLineRenderers()
    {
        foreach (Transform child in transform)
        {
            LineRenderer[] linerenderes = GetComponentsInChildren<LineRenderer>(true);

            if (child.TryGetComponent(out UpgradeNode node))
            {
                node.UpdateLineRenderers();
            }
        }
    }
}
