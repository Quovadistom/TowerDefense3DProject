using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleMenuButton : MonoBehaviour
{
    [SerializeField] private MenuController m_menuController;
    [SerializeField] private MenuPage m_pageToToggle;

    private Button m_button;

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnbuttonClick);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(OnbuttonClick);
    }

    private void OnbuttonClick()
    {
        m_menuController.PushOrPopMenuPage(m_pageToToggle);
    }
}
