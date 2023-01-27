using NaughtyAttributes;
using System;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using Zenject;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Transform m_transformToMove;
    [Tag] public string[] InvalidTags;

    private TouchInputService m_touchInputService;
    private LayerSettings m_layerSettings;
    private DraggingService m_draggingService;
    private int m_invalidColliderAmount;

    private bool m_canDrag;
    public bool CanDrag
    {
        get
        {
            return m_canDrag;
        }
        set
        {
            if (value == true && m_draggingService.IsDraggingInProgress)
            {
                return;
            }

            m_draggingService.IsDraggingInProgress = value;
            m_canDrag = value;
        }
    }

    public event Action TowerPlaced;
    public event Action InvalidPlacementDetected;
    public event Action ValidPlacementDetected;
    public event Action PlacementRequested;

    [Inject]
    public void Construct(TouchInputService touchInputService, LayerSettings layerSettings, DraggingService draggingService)
    {
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
        m_draggingService = draggingService;
    }

    private void Update()
    {
        if (CanDrag && m_touchInputService.TryGetTouchPhase(out TouchPhase touchPhase))
        {
            if (touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Moved)
            {
                if (m_touchInputService.TryGetRaycast(m_layerSettings.GameBoardLayer, out RaycastHit hit))
                {
                    m_transformToMove.position = new Vector3(hit.point.x, 0, hit.point.z);
                }
            }
            else if (m_invalidColliderAmount == 0 && touchPhase == TouchPhase.Ended)
            {
                PlacementRequested?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (InvalidTags.Contains(other.tag))
        {
            InvalidPlacementDetected?.Invoke();
            m_invalidColliderAmount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (InvalidTags.Contains(other.tag))
        {
            m_invalidColliderAmount--;

            if (m_invalidColliderAmount == 0)
            {
                ValidPlacementDetected?.Invoke();
            }
        }
    }
}
