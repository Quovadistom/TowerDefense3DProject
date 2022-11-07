

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TargetingDropDown : MonoBehaviour
{
    public SelectedTurretMenu SelectedTurretMenu;

    private TMP_Dropdown m_dropdown;
    private List<ITargetMethod> m_targetingMethods = new List<ITargetMethod>();

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
        var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetInterfaces().Contains(typeof(ITargetMethod)));

        foreach (var type in types)
        {
            m_targetingMethods.Add((ITargetMethod)Activator.CreateInstance(type));
        }

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (ITargetMethod method in m_targetingMethods)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = method.Name;
            options.Add(optionData);
        }

        m_dropdown.AddOptions(options);
    }


    private void OnTurretChanged(TurretInfoComponent selectedTurret)
    {
        if (selectedTurret != null && selectedTurret.gameObject.TryGetComponent(out TurretTargetingComponent turretTargetingComponent))
        {
            m_dropdown.value = m_targetingMethods.IndexOf(m_targetingMethods.First(x => x.GetType() == turretTargetingComponent.CurrentTargetMethod.GetType()));
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
            turretTargetingComponent.CurrentTargetMethod = m_targetingMethods[index];
        }
    }
}
