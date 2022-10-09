using UnityEngine;

public class Swappable<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] T m_objectToSwap;

    public virtual void SwapVisuals(T newVisuals)
    {
        Destroy(m_objectToSwap);
        m_objectToSwap = Instantiate(newVisuals, this.transform);
    }
}