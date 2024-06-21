using System;
using DG.Tweening;
using UnityEngine;

public class TurretCloseRange : TurretBarrel
{
    public DamageModule DamageModule;

    [SerializeField] private float m_conversionFactor;
    [SerializeField] private Transform m_sphereCollider;
    [SerializeField] private Transform m_neck;

    public event Action NeckExtended;
    public event Action NeckRetracted;

    private Vector3 m_basePosition;
    private Vector3 m_baseNeckPosition;
    private Sequence m_sequence;
    private Vector3 m_targetPosition;
    private float m_speed;
    private bool m_allowAttack = false;

    protected override void Awake()
    {
        base.Awake();

        m_basePosition = m_sphereCollider.localPosition;
        m_baseNeckPosition = m_neck.localPosition;
    }

    protected override void Update()
    {
        base.Update();

        m_neck.localPosition = m_baseNeckPosition - Vector3.Lerp(Vector3.zero, m_basePosition - m_sphereCollider.transform.localPosition, 0.6f);
        m_neck.localScale = new Vector3(0.0001f + ((m_basePosition.z - m_sphereCollider.localPosition.z) * 0.00062f), 0.0001f, 0.0001f);

        if (CurrentTarget != null)
        {
            var targetDistance = Vector3.Distance(m_sphereCollider.localPosition,
                        m_sphereCollider.parent.InverseTransformPoint(CurrentTarget.EnemyMiddle.transform.position));

            m_targetPosition = new Vector3(m_sphereCollider.localPosition.x,
                m_sphereCollider.localPosition.y,
                m_sphereCollider.localPosition.z - targetDistance);

            m_speed = targetDistance / 4;
            if (UpdateTarget && m_allowAttack && IsLockedOnTarget(CurrentTarget))
            {
                m_allowAttack = false;
                UpdateTarget = false;

                m_sequence?.Kill();
                m_sequence = DOTween.Sequence();

                m_sequence.Append(m_sphereCollider.DOLocalMove(m_targetPosition, m_speed).SetEase(Ease.InSine))
                    .AppendCallback(() => NeckExtended?.Invoke())
                    .Append(RetractAnim());
            }
        }
    }

    public void Retract()
    {
        m_sequence?.Kill();
        m_sequence = DOTween.Sequence();
        m_sequence = RetractAnim();
    }

    private Sequence RetractAnim()
    {
        ResetTimer();
        return DOTween.Sequence().Append(m_sphereCollider.DOLocalMove(m_basePosition, 0.5f))
           .AppendCallback(() =>
           {
               UpdateTarget = true;
               NeckRetracted?.Invoke();
           });
    }

    public override void TimeElapsed(BasicEnemy currentTarget)
    {
        m_allowAttack = true;
    }
}
