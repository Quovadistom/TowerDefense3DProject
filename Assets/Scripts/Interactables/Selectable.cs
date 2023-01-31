using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Interactables
{
    public class Selectable : MonoBehaviour
    {
        public GameObject GameObjectToSelect;
        public GameObject VisualsToShow;
        private ColorSettings m_colorSettings;

        public bool IsSelected { get; private set; } = false;
        public bool IsSelectedAgain { get; private set; } = false;

        public event Action ObjectSelected;
        public event Action SelectedAgain;
        public event Action<Selectable> Destroyed;

        [Inject]
        public void Construct(ColorSettings colorSettings)
        {
            m_colorSettings = colorSettings;
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }

        public void OutlineObject(bool enabled, Color color)
        {
            foreach (Outline outline in GameObjectToSelect.GetComponentsInChildren<Outline>())
            {
                outline.OutlineColor = color;
                outline.enabled = enabled;
            }
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;

            if (!selected)
            {
                IsSelectedAgain = false;
            }

            OutlineObject(selected, m_colorSettings.DefaultOutline);

            if (VisualsToShow != null)
            {
                VisualsToShow.SetActive(selected);
            }

            if (selected)
            {
                ObjectSelected?.Invoke();
            }
        }

        public void ClickAgain()
        {
            IsSelectedAgain = true;
            SelectedAgain?.Invoke();
        }
    }
}