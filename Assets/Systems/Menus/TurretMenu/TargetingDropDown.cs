using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class TargetingDropDown : MonoBehaviour
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


    private void OnTurretChanged(TowerModule selectedTurret)
    {
        if (selectedTurret == null)
        {
            return;
        }

        bool success = !selectedTurret.TryFindAndActOnModule<TargetMethodModule>((component) =>
        {
            m_dropdown.value = m_dropdown.options.Select((info, index) => (info, index)).First(x => x.info.text == component.TargetMethod.Name).index;
        });

        m_dropdown.gameObject.SetActive(success);
    }

    private void OnTargetingChanged(int index)
    {
        SelectedTurretMenu.SelectedTurret.TryFindAndActOnModule<TargetMethodModule>((component) =>
        component.TargetMethod = m_turretCollection.TargetMethodList.First(x => x.Name == m_dropdown.options[index].text));
    }
}
