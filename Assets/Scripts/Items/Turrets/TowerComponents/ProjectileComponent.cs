using System;

[Serializable]
public class ProjectileComponent : ComponentBase
{
    public ComponentDataTypeWithEvent<ProjectileBase> BulletPrefab;
    public ComponentDataTypeWithEvent<float> BulletSpeed;
    public ComponentDataTypeWithEvent<float> EplosionRange;
}
