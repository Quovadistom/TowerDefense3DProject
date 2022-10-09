using UnityEngine;

[CreateAssetMenu(fileName = "LayerSettings", menuName = "ScriptableObjects/LayerSettings")]
public class LayerSettings : ScriptableObject
{
    public LayerMask GameBoardLayer;
    public LayerMask SelectableLayer;
    public LayerMask UILayer;
    public LayerMask EnemyLayer;
}
