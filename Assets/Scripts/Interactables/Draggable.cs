using Assets.Scripts.Interactables;
using System.ComponentModel.Design;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Selectable))]
public class Draggable : MonoBehaviour
{
    public Transform TransformToMove;
    public RangeVisualiser RangeVisualiser;
    [SerializeField] private CostComponent m_costComponent;

    private SelectionService m_selectionService;
    private TouchInputService m_touchInputService;
    private LayerSettings m_layerSettings;
    private ColorSettings m_colorSettings;
    private PlacementService m_placementService;
    private int m_pathColliderAmount;
    private bool m_isPlaced = false;

    private bool m_allowDragging { get; set; } = true;

    [Inject]
    public void Construct(SelectionService selectionService, TouchInputService touchInputService, LayerSettings layerSettings, ColorSettings colorSettings, PlacementService placementService)
    {
        m_selectionService = selectionService;
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
        m_colorSettings = colorSettings;
        m_placementService = placementService;
    }

    private void Awake()
    {
        m_selectionService.ForceClearSelected();
        m_placementService.IsPlacementInProgress = true;
    }

    void Update()
    {     
        if (m_touchInputService.TryGetTouchPhase(out TouchPhase touchPhase))
        {
            if (m_allowDragging && (touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Moved))
            {
                if (m_touchInputService.TryGetRaycast(m_layerSettings.GameBoardLayer, out RaycastHit hit))
                {
                    TransformToMove.position = new Vector3(hit.point.x, 0, hit.point.z);
                }
            }

            if (!m_isPlaced && m_pathColliderAmount == 0 && (touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Canceled))
            {
                m_isPlaced = true;
                m_allowDragging = false;
                m_placementService.IsPlacementInProgress = false;
                m_costComponent.SubtractCost();
                m_selectionService.ForceSetSelected(this.transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Path") || other.CompareTag("Turret"))
        {
            RangeVisualiser.SetRangeColor(m_colorSettings.RangeBlockedToPlaceColor);
            m_pathColliderAmount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Path") || other.CompareTag("Turret"))
        {
            m_pathColliderAmount--;
            if (m_pathColliderAmount == 0)
            {
                RangeVisualiser.SetRangeColor(m_colorSettings.RangeFreeToPlaceColor);
            }
        }
    }
}
