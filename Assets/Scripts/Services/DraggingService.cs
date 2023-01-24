using System;

public class DraggingService
{
    public event Action<bool> PlacementProgressChanged;
    private bool m_isPlacementInProgress;
    public bool IsDraggingInProgress
    {
        get => m_isPlacementInProgress;
        set
        {
            m_isPlacementInProgress = value;
            PlacementProgressChanged?.Invoke(m_isPlacementInProgress);
        }
    }
}
