using System;

public class UpgradeContainer<T> : Upgrade<T> where T : ComponentBase
{
    private Action<T> m_componentAction = null;
    public override Action<T> ComponentAction => m_componentAction;

    public UpgradeContainer(Action<T> componentAction)
    {
        m_componentAction = componentAction;
    }
}

