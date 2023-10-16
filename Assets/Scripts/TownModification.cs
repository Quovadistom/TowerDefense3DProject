using UnityEngine;

public abstract class TownModification<T> : Modification<T> where T : ModuleBase
{
    public GameObject ModificationVisual;
}