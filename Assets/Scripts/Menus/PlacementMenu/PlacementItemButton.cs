using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlacementItemButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_titleText;
    [SerializeField] private Button m_button;

    public void InitializeButton(string text, UnityAction onClickAction)
    {
        m_titleText.text = text;
        m_button.onClick.AddListener(onClickAction);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
    }
}
