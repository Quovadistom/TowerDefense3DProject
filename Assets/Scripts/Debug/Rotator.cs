using System;
using UnityEngine;

[Flags]
public enum Rotation
{
    None = 0,
    X = 1,
    Y = 2,
    Z = 4
}

public class Rotator : MonoBehaviour
{
    //Rotational Speed
    [SerializeField] private float m_speed = 0f;

    [SerializeField] private Rotation m_rotation;

    void Update()
    {
        //Forward Direction
        if (m_rotation.HasFlag(Rotation.X))
        {
            transform.Rotate(Time.deltaTime * m_speed, 0, 0);
        }
        if (m_rotation.HasFlag(Rotation.Y))
        {
            transform.Rotate(0, Time.deltaTime * m_speed, 0);
        }
        if (m_rotation.HasFlag(Rotation.Z))
        {
            transform.Rotate(0, 0, Time.deltaTime * m_speed);
        }
    }
}