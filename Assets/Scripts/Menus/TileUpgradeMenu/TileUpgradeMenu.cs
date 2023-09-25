using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TileUpgradeMenu : MonoBehaviour
{
    [SerializeField] private MenuController m_menuController;
    [SerializeField] private MenuPage m_menuPage;

    [SerializeField] private RectTransform m_container;
    [SerializeField] private Button m_nextTile;
    [SerializeField] private Button m_previousTile;
    [SerializeField] private Slider m_slider;

    private float m_tileItemWidth;
    private TownHousingService m_townHousingService;

    [Inject]
    private void Construct(TownHousingService townHousingService)
    {
        m_townHousingService = townHousingService;
    }

    private void Awake()
    {
        m_townHousingService.TileHousingUpgradeRequested += OnTileUpgradeRequested;
        m_nextTile.onClick.AddListener(OnNextTile);
        m_previousTile.onClick.AddListener(OnPreviousTile);
        m_slider.onValueChanged.AddListener(OnSliderValueChanged);

        m_tileItemWidth = m_container.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
    }

    private void OnEnable()
    {
        m_container.localPosition = new Vector3(-m_tileItemWidth / 2, 0, 0);
        m_slider.maxValue = m_container.childCount - 1;
    }

    private void OnDestroy()
    {
        m_townHousingService.TileHousingUpgradeRequested -= OnTileUpgradeRequested;
        m_nextTile.onClick.RemoveListener(OnNextTile);
        m_previousTile.onClick.RemoveListener(OnPreviousTile);
        m_slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnTileUpgradeRequested(HousingData obj)
    {
        m_menuController.PushMenuPage(m_menuPage);
    }

    private void OnPreviousTile()
    {
        float containerPosition = Mathf.Clamp(m_container.localPosition.x + m_tileItemWidth,
            (-m_tileItemWidth / 2) - (m_container.childCount - 1) * m_tileItemWidth,
            -m_tileItemWidth / 2);

        m_container.DOLocalMoveX(containerPosition, 0.2f).SetEase(Ease.InOutQuint);

        m_slider.value--;
    }

    private void OnNextTile()
    {
        float containerPosition = Mathf.Clamp(m_container.localPosition.x - m_tileItemWidth,
            (-m_tileItemWidth / 2) - (m_container.childCount - 1) * m_tileItemWidth,
            -m_tileItemWidth / 2);

        m_container.DOLocalMoveX(containerPosition, 0.2f).SetEase(Ease.InOutQuint);
        m_slider.value++;
    }

    private void OnSliderValueChanged(float value)
    {
        float containerPosition = value * -m_tileItemWidth - 400;
        m_container.DOLocalMoveX(containerPosition, 0.2f).SetEase(Ease.InOutQuint);
    }
}
