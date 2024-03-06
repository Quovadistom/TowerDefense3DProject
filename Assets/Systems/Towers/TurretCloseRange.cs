using DG.Tweening;
using UnityEngine;

public class TurretCloseRange : TurretBarrel
{
    [SerializeField] private float m_conversionFactor;
    [SerializeField] private Transform m_sphereCollider;
    [SerializeField] private SkinnedMeshRenderer m_skinnedMeshRenderer;

    public override float Interval => 0.5f; //Component.Firerate;
    public bool IsMovingToTarget = false;

    private Vector3 m_basePosition;
    private Sequence m_sequence;

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

    public void Retract()
    {
        m_sequence?.Kill();
        m_sequence = DOTween.Sequence();

        IsMovingToTarget = false;

        m_sequence.Append(m_sphereCollider.DOLocalMove(m_basePosition, 0.5f / 5)).
            AppendCallback(() => UpdateAndFollowTarget = true);
    }

    public override void TimeElapsed(BasicEnemy basicEnemy)
    {
        m_sequence?.Kill();
        m_sequence = DOTween.Sequence();
        UpdateAndFollowTarget = false;
        IsMovingToTarget = true;

        m_sequence.Append(m_sphereCollider.DOMove(basicEnemy.EnemyMiddle.position, 0.5f / 5)).
            AppendCallback(() => IsMovingToTarget = false).
            Append(m_sphereCollider.DOLocalMove(m_basePosition, 0.5f / 5)).
            AppendCallback(() => UpdateAndFollowTarget = true);
    }
}
