using System;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public class Selectable : MonoBehaviour
    {
        public GameObject GameObjectToSelect;
        public GameObject VisualsToShow;

        public bool IsSelected { get; private set; }

        public event Action SelectedAgain;

        public void SetSelected(bool selected)
        {
            IsSelected = selected;

            foreach (Outline outline in GameObjectToSelect.GetComponentsInChildren<Outline>())
            {
                outline.enabled = selected;
            }

            if (VisualsToShow != null)
            {
                VisualsToShow.SetActive(selected);
            }
        }

        public void ClickAgain()
        {
            SelectedAgain?.Invoke();
        }
    }
}