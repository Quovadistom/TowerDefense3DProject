using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameBoostSelectionButton : MonoBehaviour
{
    [SerializeField] UpgradeMenu m_upgradeMenu;
    [SerializeField] Button m_button;
    [SerializeField] TMP_Text m_titleText;
    private BoostAvailabilityService m_boostAvailabilityService;
    private GameBoostService m_gameBoostService;

    private int m_index;

    [Inject]
    public void Construct(BoostAvailabilityService boostAvailabilityService, GameBoostService gameBoostService)
    {
        m_boostAvailabilityService = boostAvailabilityService;
        m_gameBoostService = gameBoostService;
    }

    private void Awake()
    {
        m_button.onClick.AddListener(OnButtonClicked);
        m_index = transform.GetSiblingIndex();

        //if (m_boostAvailabilityService.TryGetGameBoostInformation(m_gameBoostService.GameBoosts.ElementAt(m_index), out GameUpgradeBase gameBoostBase))
        //{
        //    SetButtonInfo(gameBoostBase.name);
        //}

        m_gameBoostService.GameBoostActivated += OnGameBoostActivated;
    }

    private void OnGameBoostActivated(int index, Guid title)
    {
        if (m_index == index)
        {
            SetButtonInfo(title.ToString());
        }
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(OnButtonClicked);
    }

    private void SetButtonInfo(string title)
    {
        m_titleText.text = title;
    }

    private void OnButtonClicked()
    {
        List<ButtonInfo> buttonInfos = new();

        foreach (KeyValuePair<BoostContainer, int> boost in m_boostAvailabilityService.GetAvailableBoostList().Where(boost => boost.Key.TargetObjectID != Guid.Empty))
        {
            for (int i = 0; i < boost.Value; i++)
            {
                buttonInfos.Add(new ButtonInfo()
                {
                    Title = boost.Key.Name,
                    Callback = () =>
                    {
                        m_gameBoostService.AddBoost(m_index, boost.Key);
                        m_upgradeMenu.CloseItemMenu();
                    }
                });
            }
        }

        m_upgradeMenu.OpenItemMenu(buttonInfos);
    }
}
