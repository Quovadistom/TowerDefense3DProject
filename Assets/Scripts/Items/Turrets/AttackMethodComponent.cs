using System;

public class AttackMethodComponent : ChangeVisualComponent
{
    private IAttackMethod m_currentAttackMethod;
    public event Action<IAttackMethod> AttackMethodChanged;

    public IAttackMethod CurrentAttackMethod
    {
        get => m_currentAttackMethod;
        set
        {
            m_currentAttackMethod = value;
            AttackMethodChanged?.Invoke(m_currentAttackMethod);
        }
    }
}
