using System;
using System.Collections.Generic;

[Serializable]
public class TowerModificationTreeRow
{
    public List<TowerModificationData> TowerModifications;

    public void CopyTreeData(TowerModificationTreeData treeToCopy, TowerModule towerInfoComponent)
    {
        foreach (TowerModificationData towerModificationData in TowerModifications)
        {
            towerModificationData.CopyTreeData(treeToCopy, towerInfoComponent);
        }
    }
}

