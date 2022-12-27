using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SellButton : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private SelectedTurretMenu m_selectedTurretMenu;
    private DifficultyService m_difficultyService;
    private LevelService m_levelService;
    private TowerService m_towerService;

    [Inject]
    public void Construct(DifficultyService difficultyService, LevelService levelService, TowerService towerService)
    {
        m_difficultyService = difficultyService;
        m_levelService = levelService;
        m_towerService = towerService;
    }

    private void Awake()
    {
        m_button.onClick.AddListener(OnButtonClick);
    }    
    
    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        m_levelService.Money += m_selectedTurretMenu.SelectedTurret.Value; // Times a sellback factor?
        m_towerService.RemoveTower(m_selectedTurretMenu.SelectedTurret);
        Destroy(m_selectedTurretMenu.SelectedTurret.gameObject);
    }
}
