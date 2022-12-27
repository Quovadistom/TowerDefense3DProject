using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class MoneyCounter : MonoBehaviour
{
    public TMP_Text m_text;
    private LevelService m_levelService;

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    private void Awake()
    {
        OnMoneyChanged(m_levelService.Money);
        m_levelService.MoneyChanged += OnMoneyChanged;
    }

    private void OnDestroy()
    {
        m_levelService.MoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(int currentMoney)
    {
        m_text.text = currentMoney.ToString();
    }
}
