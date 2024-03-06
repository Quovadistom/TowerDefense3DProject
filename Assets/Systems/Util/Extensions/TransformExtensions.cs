using UnityEngine;

public static class TransformExtensions
{
    public static Vector2 HorizontalPosition(this Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public static Transform ClearChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
}
