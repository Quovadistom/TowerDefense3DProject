using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_renderer;
    [SerializeField] private float m_distanceFactor = 100;
    [SerializeField] private float m_angleFactor = 10;

    private Vector3 m_oldPosition;
    private float m_oldAngle;

    private void Awake()
    {
        m_oldPosition = transform.position;
        m_oldAngle = transform.eulerAngles.y;

        m_renderer.SetBlendShapeWeight(0, 50);
        m_renderer.SetBlendShapeWeight(1, 100);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(m_oldPosition, transform.position);
        float angle = m_oldAngle - transform.eulerAngles.y;

        float blendShapeWeightLeft = m_renderer.GetBlendShapeWeight(0);
        float blendShapeWeightRight = m_renderer.GetBlendShapeWeight(1);

        float valueLeft = blendShapeWeightLeft <= 0 ? 100 : blendShapeWeightLeft - distance * m_distanceFactor - angle * m_angleFactor;
        float valueRight = blendShapeWeightRight <= 0 ? 100 : blendShapeWeightRight - distance * m_distanceFactor - angle * m_angleFactor;


        m_renderer.SetBlendShapeWeight(0, valueLeft);
        m_renderer.SetBlendShapeWeight(1, valueRight);

        m_oldPosition = transform.position;
        m_oldAngle = transform.eulerAngles.y;
    }
}
