using Assets.Scripts.Interactables;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class SelectionService : ITickable
{
    private TouchInputService m_touchInputService;
    private LayerSettings m_layerSettings;

    private Selectable m_selected;

    public bool LockSelection { get; set; }

    public event Action<Component> ObjectSelected;

    public SelectionService(TouchInputService touchInputService, LayerSettings layerSettings)
    {
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
    }

    public void Tick()
    {
        if (m_touchInputService.IsSingleTouchActive())
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                if (!LockSelection && m_touchInputService.TryGetRaycast(m_layerSettings.SelectableLayer, out RaycastHit hit))
                {
                    Selectable selected = hit.transform.GetComponent<Selectable>();
                    SetSelected(selected);
                }
                else if (!LockSelection && !IsPointerOverUIObject())
                {
                    SetSelected(null);
                }
            }
        }
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void SetSelected(Selectable selectable)
    {
        SetSelectedState(false);

        m_selected = selectable;

        SetSelectedState(true);

        Component selectedComponent = m_selected == null ? null : m_selected.ComponentToSelect;
        ObjectSelected?.Invoke(selectedComponent);
    }

    private void SetSelectedState(bool state)
    {
        if (m_selected != null)
        {
            m_selected.SetSelected(state);
        }
    }

    public void ForceClearSelected()
    {
        SetSelected(null);
    }

    public void ForceSetSelected(Transform transformToSelect)
    {
        Selectable selectable = transformToSelect.GetComponentInChildren<Selectable>();

        if (selectable != null)
        {
            SetSelected(selectable);
        }
        else
        {
            Debug.LogWarning($"No Selectable component found on {transformToSelect.name}");
        }
    }
}
