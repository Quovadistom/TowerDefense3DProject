using DG.Tweening;
using UnityEngine;

public class TurretCloseRange : TurretBarrel<TurretCloseRangeComponent>
{
    [SerializeField] private float m_conversionFactor;
    [SerializeField] private Transform m_sphereCollider;
    [SerializeField] private SkinnedMeshRenderer m_skinnedMeshRenderer;

    public override float Interval => Component.Firerate;

    private Vector3 m_basePosition;

    protected override void Awake()
    {
        base.Awake();

        m_basePosition = m_sphereCollider.transform.localPosition;
    }

    protected override void Update()
    {
        base.Update();

        float distance = Vector3.Distance(m_basePosition, m_sphereCollider.transform.localPosition);
        m_skinnedMeshRenderer.SetBlendShapeWeight(0, distance * m_conversionFactor);
    }

    public override void TimeElapsed(BasicEnemy basicEnemy)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(m_sphereCollider.DOMove(basicEnemy.EnemyMiddle.position, Component.Firerate / 5)).
            Append(m_sphereCollider.DOLocalMove(m_basePosition, Component.Firerate / 5));
    }
}
