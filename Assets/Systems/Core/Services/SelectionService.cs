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
    private DraggingService m_draggingService;
    private Selectable m_selected;

    public bool LockSelection { get; set; }

    /// <summary>
    /// Event that fires when a gameobject is selected.
    /// </summary>
    public event Action<Selectable> GameObjectSelected;
    /// <summary>
    /// Event that fires when a gameobject is clicked, but was already selected.
    /// </summary>
    public event Action<Selectable> GameObjectClickedAgain;
    /// <summary>
    /// Event that fires when the selection is locked to a specific gameobject, but another gameobject is clicked on
    /// </summary>
    public event Action<Selectable> GameObjectClickedWhileSelectionLocked;

    public SelectionService(TouchInputService touchInputService, DraggingService draggingService, LayerSettings layerSettings)
    {
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
        m_draggingService = draggingService;
    }

    public void Tick()
    {
        if (m_touchInputService.IsSingleTouchActive())
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                if (m_touchInputService.TryGetRaycast(m_layerSettings.SelectableLayer, out RaycastHit hit))
                {
                    Selectable selected = hit.transform.GetComponent<Selectable>();
                    if (!LockSelection)
                    {
                        SetSelected(selected);
                    }
                    else if (LockSelection)
                    {
                        GameObjectClickedWhileSelectionLocked?.Invoke(selected);
                    }
                }
                else if (!m_draggingService.IsDraggingInProgress && !IsPointerOverUIObject())
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

    private void SetSelected(Selectable selectable, bool silent = false)
    {
        if (selectable != null && m_selected == selectable)
        {
            selectable.ClickAgain();
            GameObjectClickedAgain?.Invoke(selectable);
            return;
        }

        SetSelectedState(false);

        m_selected = selectable;

        SetSelectedState(true);

        if (!silent)
        {
            GameObjectSelected?.Invoke(selectable);
        }
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
        LockSelection = false;
        SetSelected(null);
    }

    /// <summary>
    /// Sets the object to selected without triggering GameObjectSelected.
    /// </summary>
    /// <param name="selectable"></param>
    public void ForceSetSelectedSilent(Transform transformToSelect) => ForceSetSelected(transformToSelect, true);

    public void ForceSetSelected(Transform transformToSelect, bool silent = false)
    {
        LockSelection = false;

        Selectable selectable = transformToSelect.GetComponentInChildren<Selectable>();

        if (selectable != null)
        {
            SetSelected(selectable, silent);
        }
        else
        {
            Debug.LogWarning($"No Selectable component found on {transformToSelect.name}");
        }
    }
}
