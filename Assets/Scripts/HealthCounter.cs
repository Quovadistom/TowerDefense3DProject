using TMPro;
using Zenject;

public class HealthCounter : ModuleWithModificationBase
{
    public TMP_Text m_text;
    private LevelService m_levelService;

    public HealthModule HealthComponent;

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    protected void Awake()
    {
        OnHealthComponentUpdated(HealthComponent.Health);
        HealthComponent.OnHealthChange += OnHealthComponentUpdated;
    }

    private void OnDestroy()
    {
        HealthComponent.OnHealthChange -= OnHealthComponentUpdated;
    }

    private void OnHealthComponentUpdated(int health)
    {
        if (health == 0)
        {
            m_levelService.SetGameOver();
        }
        else if (health < 0)
        {
            return;
        }

        m_text.text = health.ToString();
    }
}
