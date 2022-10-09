using System;
using System.ComponentModel.Design;
using UnityEngine;
using Zenject;

public class Selectable : MonoBehaviour
{
    public Component ComponentToSelect;
    public Outline Outline;
    public GameObject VisualsToShow;

    public bool IsSelected { get; private set; }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;

        if (Outline != null)
        {
            Outline.enabled = selected;
        }

        if (VisualsToShow != null)
        {
            VisualsToShow.SetActive(selected);
        }
    }
}
