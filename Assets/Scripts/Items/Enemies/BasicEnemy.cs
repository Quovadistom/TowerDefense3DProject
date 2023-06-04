using UnityEngine;
using Zenject;

public class BasicEnemy : Poolable
{
    public float Speed = 5f;
    public int StartingHealth = 100;
    public Transform EnemyMiddle;
    [SerializeField] MeshRenderer m_meshRenderer;
    [SerializeField] private int m_enemyWorth;

    private int m_waypointIndex = 0;
    private Transform m_target;
    private LevelService m_levelService;
    private WaveService m_waveService;
    private BoostService m_boostService;
    private float m_currentHealth;

    [Inject]
    public void Construct(LevelService levelService, WaveService waveService, BoostService boostService)
    {
        m_levelService = levelService;
        m_waveService = waveService;
        m_boostService = boostService;
    }

    private void Awake()
    {
        ResetObject();
    }

    private void OnEnable()
    {
        SetWaypoint(m_waypointIndex);
    }

    private void Update()
    {
        Vector3 direction = m_target.position - transform.position;
        transform.Translate(direction.normalized * Speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, m_target.position) <= 0.02f)
        {
            m_waypointIndex++;
            SetWaypoint(m_waypointIndex);
        }
    }

    private void SetWaypoint(int waypointIndex)
    {
        if (m_levelService.Waypoints.Count > waypointIndex)
        {
            m_target = m_levelService.Waypoints[waypointIndex];
        }
        else
        {
            UpgradeContainer<HealthComponent> upgradeContainer = new UpgradeContainer<HealthComponent>((component) =>
            {
                component.Health--;
            });

            m_boostService.SendBoost(upgradeContainer);

            m_waveService.AliveEnemies--;
            m_poolingService.ReturnPooledObject(this);
        }
    }

    public override void ResetObject()
    {
        base.ResetObject();
        m_waypointIndex = 0;
        m_currentHealth = StartingHealth;
        m_meshRenderer.material.color = Color.red;
    }

    public void TakeDamage(float damage)
    {
        m_currentHealth -= Mathf.Max(0, damage);
        m_meshRenderer.material.color = new Color(1, m_meshRenderer.material.color.g + damage / StartingHealth, 0, 1);
        if (m_currentHealth <= 0)
        {
            m_levelService.Money += m_enemyWorth;
            m_waveService.AliveEnemies--;
            m_poolingService.ReturnPooledObject(this);
            ResetObject();
        }
    }
}