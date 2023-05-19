using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class StatusEffectDropDown : MonoBehaviour
{
    public SelectedTurretMenu SelectedTurretMenu;

    private TMP_Dropdown m_dropdown;
    private TurretCollection m_turretCollection;

    [Inject]
    private void Construct(TurretCollection turretCollection)
    {
        m_turretCollection = turretCollection;
    }

    void Awake()
    {
        m_dropdown = GetComponent<TMP_Dropdown>();

        FillDropdown();
        m_dropdown.onValueChanged.AddListener(OnTargetingChanged);
        SelectedTurretMenu.TurretDataChanged += OnTurretChanged;
    }

    private void OnDestroy()
    {
        m_dropdown.onValueChanged.RemoveAllListeners();
        SelectedTurretMenu.TurretDataChanged -= OnTurretChanged;
    }

    private void FillDropdown()
    {
        List<TMP_Dropdown.OptionData> options = new();
        foreach (ITargetMethod method in m_turretCollection.TargetMethodList)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData
            {
                text = method.Name
            };
            options.Add(optionData);
        }

        m_dropdown.AddOptions(options);
    }


    private void OnTurretChanged(TowerInfoComponent selectedTurret)
    {
        if (selectedTurret != null && selectedTurret.gameObject.TryGetComponent(out TurretTargetingComponent turretTargetingComponent))
        {
            m_dropdown.value = m_dropdown.options.Select((info, index) => (info, index)).First(x => x.info.text == turretTargetingComponent.CurrentTargetMethod.Name).index;
            m_dropdown.gameObject.SetActive(true);
        }
        else
        {
            m_dropdown.gameObject.SetActive(false);
        }
    }

    private void OnTargetingChanged(int index)
    {
        if (SelectedTurretMenu.SelectedTurret.gameObject.TryGetComponent(out TurretTargetingComponent turretTargetingComponent))
        {
            turretTargetingComponent.CurrentTargetMethod = m_turretCollection.TargetMethodList.First(x => x.Name == m_dropdown.options[index].text);
        }
    }
}
