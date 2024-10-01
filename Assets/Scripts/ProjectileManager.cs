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

    // public void SpawnPlayerProjectile(Transform parent, int damage, float speed = 10f)
    // {
    //     if (playerProjectilePrefab is null)
    //         LoadResources();
    //     var obj = Instantiate(playerProjectilePrefab);
    //     var ctrl = obj.GetComponent<PrototypeCat.Projectile.Controller>();
    //     ctrl.damage = damage;
    //     ctrl.rotation = parent.rotation;
    //     obj.transform.position = new(parent.position.x, transform.position.y, transform.position.z);
    // }

    public void SpawnEnemyProjectile(Vector3 position, Quaternion rotation, int damage)
    {
        Debug.Log("Spawning projectile");
        if (m_enemyProjectilePrefab is null)
            LoadResources();
        var obj = Instantiate(m_enemyProjectilePrefab, position, rotation);
        // expensive, object pooling should be used instead
        var ctrl = obj.GetComponent<BadguyCat.Projectile.Controller>();
        ctrl.damage = damage;
        // update euler angle y
        // ctrl.rotation = Quaternion.Euler(ctrl.transform.eulerAngles.x, eulerY, ctrl.transform.eulerAngles.z);
        // ctrl.transform.position = new(position.x, position.y, position.z);
    }

    private void LoadResources()
    {
        m_enemyProjectilePrefab = Resources.Load<GameObject>("BadguyProjectile");
        m_playerProjectilePrefab = Resources.Load<GameObject>("PrototypeProjectile");
    }
}
