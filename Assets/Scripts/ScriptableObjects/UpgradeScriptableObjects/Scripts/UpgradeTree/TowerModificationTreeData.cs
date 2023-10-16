using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TowerModificationTreeData
{
    public List<TowerModificationTreeRow> Structure;

    public void Initialize()
    {
        foreach (TowerModificationTreeRow towerModificationTreeRow in Structure)
        {
            for (int i = 0; i < towerModificationTreeRow.TowerModifications.Count; i++)
            {
                TowerModificationData towerModificationData = towerModificationTreeRow.TowerModifications[i];

                if (i + 1 < towerModificationTreeRow.TowerModifications.Count)
                {
                    towerModificationData.RequiredFor.Add(towerModificationTreeRow.TowerModifications[i + 1].TowerModificationID.ToString());
                }

                foreach (TowerModificationData modificationData in GetTowerModificationsDatas(towerModificationData.RequiredFor.Select(id => Guid.Parse(id))))
                {
                    modificationData.UnlockSignals++;
                }
            }
        }
    }

    public void CopyTreeData(TowerModificationTreeData treeToCopy, TowerModule towerInfoComponent)
    {
        Initialize();
        foreach (TowerModificationTreeRow towerModificationTreeRow in Structure)
        {
            towerModificationTreeRow.CopyTreeData(treeToCopy, towerInfoComponent);
        }
    }

    public bool TryGetTowerModificationData(Guid ID, out TowerModificationData data)
    {
        data = null;

        foreach (TowerModificationTreeRow row in Structure)
        {
            data = row.TowerModifications.FirstOrDefault(modification => modification.TowerModificationID == ID);
            if (data != null)
            {
                return true;
            }
        }

        return data != null;
    }

    public IEnumerable<TowerModificationData> GetTowerModificationsDatas(IEnumerable<Guid> ids)
    {
        List<TowerModificationData> towerModificationDatas = new();

        foreach (Guid id in ids)
        {
            if (TryGetTowerModificationData(id, out TowerModificationData towerModificationData))
            {
                towerModificationDatas.Add(towerModificationData);
            }
        }

        return towerModificationDatas;
    }
}

