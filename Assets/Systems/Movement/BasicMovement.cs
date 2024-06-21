using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_renderer;
    [SerializeField] private float m_distanceFactor = 100;
    [SerializeField] private float m_angleFactor = 10;

    private Vector3 m_oldPosition;
    private Quaternion m_oldRotation;

    private void Awake()
    {
        m_oldPosition = transform.position;
        m_oldRotation = transform.rotation;

        m_renderer.SetBlendShapeWeight(0, 50);
        m_renderer.SetBlendShapeWeight(1, 100);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(m_oldPosition, transform.position);
        float angle = Quaternion.Angle(m_oldRotation, transform.rotation);

        float blendShapeWeightLeft = m_renderer.GetBlendShapeWeight(0);
        float blendShapeWeightRight = m_renderer.GetBlendShapeWeight(1);

        float movementValue = blendShapeWeightLeft - distance * m_distanceFactor;
        float rotationValue = angle * m_angleFactor;

        float valueLeft = movementValue - rotationValue;
        valueLeft = valueLeft < 0 ? 100 : valueLeft > 100 ? 0 : valueLeft;

        float compensationValue = movementValue + 50 < 100 ? 50 : -50;
        float valueRight = movementValue - compensationValue + angle * rotationValue;

        valueRight = valueRight < 0 ? 100 : valueRight > 100 ? 0 : valueRight;

        m_renderer.SetBlendShapeWeight(0, valueLeft);
        m_renderer.SetBlendShapeWeight(1, valueRight);

        m_oldPosition = transform.position;

        if (angle > 0)
        {
            m_oldRotation = transform.rotation;
        }
    }
}
