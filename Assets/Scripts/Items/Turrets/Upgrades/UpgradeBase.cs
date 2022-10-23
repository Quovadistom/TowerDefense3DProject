using System.Collections;
using UnityEngine;

public class UpgradeBase<T> : MonoBehaviour where T : TurretMediatorBase
{
    public T m_turretMediator;

    private void Reset()
    {
#if UNITY_EDITOR
        if (m_turretMediator == null)
        {
            m_turretMediator = GetComponentInParent<T>();

            if (m_turretMediator == null)
            {
                Debug.LogError($"Turret mediator {typeof(T)} not found! Destroying this object...", this);
                DestroyImmediate(gameObject);
            }
        }
#endif
    }
}