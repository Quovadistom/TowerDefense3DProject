using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_itemTitle;
    [SerializeField] private Image m_itemIcon;

    public void SetItemInfo(string title, Sprite sprite = null)
    {
        m_itemTitle.text = title;

        if (sprite != null)
        {
            m_itemIcon.sprite = sprite;
        }
    }
}
