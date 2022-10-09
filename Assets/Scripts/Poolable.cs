using UnityEngine;
using Zenject;

public class Poolable : MonoBehaviour
{
    [Inject] protected PoolingService m_poolingService; 

    public bool IsPooled { get; private set; } = false;

    public virtual void InitializeObject()
    {
        gameObject.SetActive(true);
        IsPooled = false;
    }

    public virtual void ResetObject()
    {
        gameObject.SetActive(false);
        IsPooled = true;
    }

    public class Factory : PlaceholderFactory<Poolable, Poolable>
    {
    }
}