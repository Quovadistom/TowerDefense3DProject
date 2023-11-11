using System;
using UnityEngine.SceneManagement;

public class LevelService : ServiceSerializationHandler<LevelServiceDTO>
{
    public string TargetedCoordinates { get; set; }
    public Map MapInfo { get; set; }

    private int m_health = 10;
    private int m_money = 1000;

    private ModuleModificationService m_modificationService;
    private SceneCollection m_sceneCollection;
    private TownTileService m_tileService;

    public LevelService(ModuleModificationService modificationService,
        SerializationService serializationService,
        TownTileService tileService,
        SceneCollection sceneCollection,
        DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_modificationService = modificationService;
        m_sceneCollection = sceneCollection;
        m_tileService = tileService;
    }

    public event Action<int> MoneyChanged;
    public event Action GameOverRequested;

    public int Money
    {
        get { return m_money; }
        set
        {
            m_money = value;
            MoneyChanged?.Invoke(m_money);
        }
    }

    public void SetLevelInfo(Map map, string coordinates)
    {
        MapInfo = map;
        TargetedCoordinates = coordinates;
    }

    public void SetGameOver()
    {
        GameOverRequested?.Invoke();
    }

    public void EndLevel()
    {
        m_tileService.SetTileCapture(TargetedCoordinates, true);
        SceneManager.LoadScene(m_sceneCollection.TownScene);
    }

    protected override Guid Id => Guid.Parse("7c631af4-9fff-4572-86aa-1f2178772e80");

    protected override void ConvertDtoBack(LevelServiceDTO dto)
    {
        Money = dto.Money;
    }

    protected override void ConvertDto()
    {
        Dto.Money = Money;
    }
}

[Serializable]
public class LevelServiceDTO
{
    public int Health;
    public int Money;
}
