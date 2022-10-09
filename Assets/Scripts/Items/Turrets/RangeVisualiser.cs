using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeVisualiser : MonoBehaviour
{
    public TurretBase TurretBase;
    public SpriteRenderer Renderer;

    private void OnEnable()
    {
        float range = TurretBase.TurretData.Range;
        transform.localScale = new Vector3(range * 2, range * 2, 0);
    }

    public void SetRangeColor(Color color)
    {
        Renderer.color = color;
    }
}
