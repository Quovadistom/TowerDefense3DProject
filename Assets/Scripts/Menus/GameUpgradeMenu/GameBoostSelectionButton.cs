using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameBoostSelectionButton : MonoBehaviour
{
    [SerializeField] Button m_button;
    [SerializeField] TMP_Text m_titleText;
    private BoostAvailabilityService m_boostAvailabilityService;
    private GameBoostService m_gameBoostService;
    private MenuService m_itemMenuService;

    private int m_index;

    [Inject]
    public void Construct(BoostAvailabilityService boostAvailabilityService, GameBoostService gameBoostService, MenuService itemMenuService)
    {
        m_boostAvailabilityService = boostAvailabilityService;
        m_gameBoostService = gameBoostService;
        m_itemMenuService = itemMenuService;
    }

    private void Awake()
    {
        m_button.onClick.AddListener(OnButtonClicked);
        m_index = transform.GetSiblingIndex();

        if (m_boostAvailabilityService.TryGetGameBoostInformation(m_gameBoostService.GameBoosts.ElementAt(m_index), out GameUpgradeBase gameBoostBase))
        {
            SetButtonInfo(gameBoostBase.name);
        }

        m_gameBoostService.GameBoostActivated += OnGameBoostActivated;
    }

    private void OnGameBoostActivated(int index, string title)
    {
        if (m_index == index)
        {
            SetButtonInfo(title);
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
        List<ButtonInfo> buttonInfos = new List<ButtonInfo>();

        foreach (KeyValuePair<string, int> boost in m_boostAvailabilityService.AvailableBoosts)
        {
            if (m_boostAvailabilityService.TryGetGameBoostInformation(boost.Key, out GameUpgradeBase boostInfo))
            {
                for (int i = 0; i < boost.Value; i++)
                {
                    buttonInfos.Add(new ButtonInfo()
                    {
                        Title = boostInfo.Name,
                        Callback = () => m_gameBoostService.AddBoost(m_index, boostInfo.ID)
                    });
                }
            }
        }

        m_itemMenuService.RequestItemMenu(buttonInfos);
    }
}
