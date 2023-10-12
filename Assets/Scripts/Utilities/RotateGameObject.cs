using UnityEngine;

public enum RotateAround
{
    X,
    Y,
    Z,
}

public class RotateGameObject : MonoBehaviour
{
    [SerializeField] private RotateAround m_rotationAxle;
    [SerializeField] private float m_rotationSpeed = 0.2f;

    // Update is called once per frame
    void Update()
    {
        switch (m_rotationAxle)
        {
            case RotateAround.X:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + m_rotationSpeed, transform.localEulerAngles.y, transform.localEulerAngles.z);
                return;
            case RotateAround.Y:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + m_rotationSpeed, transform.localEulerAngles.z);
                return;
            case RotateAround.Z:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + m_rotationSpeed);
                return;
        }
    }
}
