using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;
    private GameObject m_playerProjectilePrefab;
    private GameObject m_enemyProjectilePrefab;
    
    // simple Unity singleton
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (this != Instance)
        {
            Destroy(this);
        }
        LoadResources();
    }

    public void SpawnPlayerProjectile(Vector3 position, Quaternion rotation, int damage)
    {
        if (m_playerProjectilePrefab is null)
            LoadResources();
        var obj = Instantiate(m_playerProjectilePrefab, position, rotation);
        // expensive, object pooling should be used instead
        var ctrl = obj.GetComponent<PrototypeCat.Projectile.Controller>();
        ctrl.damage = damage;
    }

    public void SpawnEnemyProjectile(Vector3 position, Quaternion rotation, int damage)
    {
        if (m_enemyProjectilePrefab is null)
            LoadResources();
        var obj = Instantiate(m_enemyProjectilePrefab, position, rotation);
        // expensive, object pooling should be used instead
        var ctrl = obj.GetComponent<BadguyCat.Projectile.Controller>();
        ctrl.damage = damage;
    }

    private void LoadResources()
    {
        m_enemyProjectilePrefab = Resources.Load<GameObject>("BadguyProjectile");
        m_playerProjectilePrefab = Resources.Load<GameObject>("PrototypeProjectile");
    }
}
