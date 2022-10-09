using UnityEngine;

public static class TransformExtensions
{
    public static Vector2 HorizontalPosition(this Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
}
