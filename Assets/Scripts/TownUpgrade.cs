using UnityEngine;

public abstract class TownUpgrade<T> : Upgrade<T> where T : ComponentBase
{
    public GameObject UpgradeVisual;
}