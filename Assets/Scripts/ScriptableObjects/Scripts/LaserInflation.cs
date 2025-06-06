﻿using System;
using System.Linq;

public class LaserInflation : InflationData
{
    protected LaserInflation(Guid guid) : base(guid)
    {
    }

    public override bool IsModelParentSuitable(ModuleParent module)
    {
        return module.UpgradableModules.Any(x => x.GetType() == typeof(TurretLaserBarrel));
    }

    public override bool IsModificationSuitable(ModuleModificationBase moduleModificationBase, ModuleParent moduleParent)
    {
        if (IsModelParentSuitable(moduleParent))
        {
            return moduleModificationBase is TowerRangeModification ||
                moduleModificationBase is TowerDamageModification;
        }

        return false;
    }
}

public class BulletInflation : InflationData
{
    protected BulletInflation(Guid guid) : base(guid)
    {
    }

    public override bool IsModelParentSuitable(ModuleParent module)
    {
        return module.UpgradableModules.Any(x => x.GetType() == typeof(TurretProjectileBarrel));
    }

    public override bool IsModificationSuitable(ModuleModificationBase moduleModificationBase, ModuleParent moduleParent)
    {
        if (IsModelParentSuitable(moduleParent))
        {
            return moduleModificationBase is TowerRangeModification ||
                moduleModificationBase is TowerDamageModification;
        }

        return false;
    }
}