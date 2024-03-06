public class EnemyService
{
    private PoolingService m_poolingService;

    public EnemyService(PoolingService poolingService)
    {
        m_poolingService = poolingService;
    }

    public BasicEnemy CreateNewEnemy(BasicEnemy enemy, WaypointList waypoints)
    {
        BasicEnemy newEnemy = (BasicEnemy)m_poolingService.GetPooledObject(enemy);
        newEnemy.SetWaypoints(waypoints.Waypoints);
        return newEnemy;
    }
}