using System;

public class PlacementService
{
    public event Action<bool> PlacementProgressChanged;
    private bool m_isPlacementInProgress;
    public bool IsPlacementInProgress
    {
        get => m_isPlacementInProgress;
        set
        {
            m_isPlacementInProgress = value;
            PlacementProgressChanged?.Invoke(m_isPlacementInProgress);
        }
    }
}
