using System;

[Serializable]
public class ProjectileModule : ModuleBase
{
    public ModuleDataTypeWithEvent<ProjectileBase> BulletPrefab;
    public ModuleDataTypeWithEvent<float> BulletSpeed;
    public ModuleDataTypeWithEvent<float> ExplosionRange;
}
