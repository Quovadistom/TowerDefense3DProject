﻿public class TargetMethodModule : ModuleBase
{
    private ITargetMethod m_targetMethod = new TargetFirstEnemy();

    public ITargetMethod TargetMethod
    {
        get { return m_targetMethod; }
        set { m_targetMethod = value; }
    }
}