using UnityEngine;

namespace Boss.Attack
{
    public class Laser : MonoBehaviour, IAttack
    {
        public Type Type => Type.Direct;
        private GameObject laserIndicator;
        private GameObject laserAttack;
        private GameObject player;
        private Player.Controller playerScript;
        private GameObject boss;
        //
        
        [SerializeField] private GameObject indicatorPrefab;
        [SerializeField] private GameObject attackPrefab;
        
        private Vector3 indicatorStartPos = new(20, 0, 0);
        private Vector3 laserStartPos = new(25, 0, 0);
        private float laserLength;
        private float indicatorTime = 1f;
        private int damage = 10;

        private void Awake()
        {
            laserIndicator = Instantiate(indicatorPrefab);
            laserAttack = Instantiate(attackPrefab);
            playerScript = FindObjectOfType<Player.Controller>();
            player = playerScript.gameObject;
            boss = FindObjectOfType<Boss.Controller>().gameObject;
            ResetLaser();
            
            laserLength = Vector3.Distance(laserAttack.transform.position, laserAttack.transform.position + laserAttack.transform.localScale / 2);
        }
    
        // the Execute method is called when the attack is executed
        public void Execute()
        {
            SetIndicatorToPlayer();
            Invoke(nameof(ShootLaser), indicatorTime);
            Invoke(nameof(ResetLaser), indicatorTime * 2);
        }

        private void SetIndicatorToPlayer()
        {
            laserIndicator.transform.position = boss.transform.position + laserLength * Vector3.Normalize(player.transform.position - boss.transform.position);
            laserIndicator.transform.LookAt(player.transform);
        }

        private void ShootLaser()
        {
            laserAttack.transform.SetPositionAndRotation(laserIndicator.transform.position, laserIndicator.transform.rotation);
            laserIndicator.transform.position = indicatorStartPos;
        }

        private void ResetLaser()
        {
            laserIndicator.transform.position = indicatorStartPos;
            laserAttack.transform.position = laserStartPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("player"))
            {
                // This should be handled with events ! ! !
                playerScript.TakeDamage(damage);
            }
        }
    }
}