using ModestTree;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotateWithTransform : MonoBehaviour
{
    [SerializeField] private Transform m_turretBarrel;

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, m_turretBarrel.transform.eulerAngles.y, 0);
    }
}
