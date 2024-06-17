using System;

[Serializable]
public class DamageModule : ModuleBase
{
    public ModuleDataTypeWithEvent<float> Damage;
    public ModuleDataTypeWithEvent<int> Piercing;
    public ModuleDataTypeWithEvent<StatusEffect> StatusEffect;
}