using System.Collections.Generic;
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
    private ResourceService m_resourceService;
    private WaveService m_waveService;
    private ModuleModificationService m_modificationService;
    private float m_currentHealth;

    private IReadOnlyList<Transform> m_waypoints;
    private float m_distanceTraveled = 0;
    private Vector3 m_oldPosition;

    public float DistanceTraveled => m_distanceTraveled;

    [Inject]
    public void Construct(ResourceService resourceService, WaveService waveService, ModuleModificationService modificationService)
    {
        m_resourceService = resourceService;
        m_waveService = waveService;
        m_modificationService = modificationService;
    }

    private void Awake()
    {
        ResetObject();
    }

    private void Update()
    {
        Vector3 direction = m_target.position - transform.position;
        transform.Translate(Speed * Time.deltaTime * direction.normalized, Space.World);
        m_distanceTraveled += Vector3.Distance(m_oldPosition, transform.position);

        if (Vector3.Distance(transform.position, m_target.position) <= 0.02f)
        {
            m_waypointIndex++;
            GoToWaypoint(m_waypointIndex);
        }

        m_oldPosition = transform.position;
    }

    public void SetWaypoints(IReadOnlyList<Transform> waypoints)
    {
        m_waypoints = waypoints;
        transform.position = waypoints[0].position;
        m_oldPosition = transform.position;
        GoToWaypoint(0);
    }

    private void GoToWaypoint(int waypointIndex)
    {
        if (m_waypoints.Count > waypointIndex)
        {
            m_target = m_waypoints[waypointIndex];
        }
        else
        {
            HealthModification modificationContainer = ScriptableObject.CreateInstance<HealthModification>();
            modificationContainer.HealthValue = -1;

            m_modificationService.SendModification(modificationContainer);

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
        m_distanceTraveled = 0;
    }

    public void TakeDamage(float damage)
    {
        m_currentHealth -= Mathf.Max(0, damage);
        m_meshRenderer.material.color = new Color(1, m_meshRenderer.material.color.g + damage / StartingHealth, 0, 1);
        if (m_currentHealth <= 0)
        {
            m_resourceService.ChangeAvailableResource<BattleFunds>(m_enemyWorth);
            m_waveService.AliveEnemies--;
            m_poolingService.ReturnPooledObject(this);
            ResetObject();
        }
    }
}