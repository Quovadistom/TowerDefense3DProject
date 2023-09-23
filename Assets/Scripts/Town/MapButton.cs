using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField] private Button m_button;

    public bool IsCompleted = false;

    private void Start()
    {
        m_button.onClick.AddListener(ButtonClicked);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
    }

    private void ButtonClicked()
    {
        if (!IsCompleted)
        {
            // Open menu for starting level
        }
    }
}
