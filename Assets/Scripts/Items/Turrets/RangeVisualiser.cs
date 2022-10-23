using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeVisualiser : MonoBehaviour
{
    public BarrelTurretMediator TurretData;
    public SpriteRenderer Renderer;
    [SerializeField] private SphereCollider m_rangeCollider;

    private void Update() // TODO: do not use update, update with event/callback
    {
        float range = TurretData.Range;
        transform.localScale = new Vector3(range * 2, range * 2, 0);
        m_rangeCollider.radius = range;
    }

    public void SetRangeColor(Color color)
    {
        Renderer.color = color;
    }
}
