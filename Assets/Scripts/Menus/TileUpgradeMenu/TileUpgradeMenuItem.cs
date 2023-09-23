using UnityEngine;

public class TileUpgradeMenuItem : MonoBehaviour
{
    [SerializeField] private RectTransform m_content;

    private RectTransform m_parent;

    private void Awake()
    {
        m_parent = transform.parent.GetComponent<RectTransform>();
    }

    private void Update()
    {

    }
}
