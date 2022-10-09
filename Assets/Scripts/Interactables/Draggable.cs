using System.ComponentModel.Design;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Selectable))]
public class Draggable : MonoBehaviour
{
    public Transform TransformToMove;
    public RangeVisualiser RangeVisualiser;
    private SelectionService m_selectionService;
    private TouchInputService m_touchInputService;
    private LayerSettings m_layerSettings;
    private ColorSettings m_colorSettings;
    private Selectable m_selectable;
    private int m_pathColliderAmount;

    public bool AllowDragging { get; set; } = true;

    [Inject]
    public void Construct(SelectionService selectionService, TouchInputService touchInputService, LayerSettings layerSettings, ColorSettings colorSettings)
    {
        m_selectionService = selectionService;
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
        m_colorSettings = colorSettings;
    }

    private void Awake()
    {
        m_selectable = GetComponent<Selectable>();
        m_selectionService.LockSelection = true;
    }

    void Update()
    {     
        if (m_selectable.IsSelected && m_touchInputService.TryGetTouchPhase(out TouchPhase touchPhase))
        {
            if (AllowDragging && (touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Moved))
            {
                if (m_touchInputService.TryGetRaycast(m_layerSettings.GameBoardLayer, out RaycastHit hit))
                {
                    TransformToMove.position = new Vector3(hit.point.x, 0, hit.point.z);
                }
            }

            if (m_pathColliderAmount == 0 && (touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Canceled))
            {
                AllowDragging = false;
                m_selectionService.LockSelection = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Path"))
        {
            RangeVisualiser.SetRangeColor(m_colorSettings.RangeBlockedToPlaceColor);
            m_pathColliderAmount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Path"))
        {
            m_pathColliderAmount--;
            if (m_pathColliderAmount == 0)
            {
                RangeVisualiser.SetRangeColor(m_colorSettings.RangeFreeToPlaceColor);
            }
        }
    }
}
