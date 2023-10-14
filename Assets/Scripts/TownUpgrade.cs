using UnityEngine;

public abstract class TownUpgrade<T> : Upgrade<T> where T : ModuleBase
{
    public GameObject UpgradeVisual;
}