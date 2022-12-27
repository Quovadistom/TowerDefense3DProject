using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SaveButton : MonoBehaviour
{
    [SerializeField] Button m_saveButton;
    private LevelService m_levelService;
    private TowerService m_turretService;

    [Inject]
    public void Construct(LevelService levelService, TowerService turretService)
    {
        m_levelService = levelService;
        m_turretService = turretService;
    }

    private void Awake()
    {
        m_saveButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        m_levelService.Save();
        m_turretService.Save();
    }

    private void OnDestroy()
    {
        m_saveButton.onClick.RemoveListener(OnButtonClicked);
    }
}
