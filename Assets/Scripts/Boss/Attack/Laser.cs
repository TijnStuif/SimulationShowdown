using UnityEngine;

namespace Boss.Attack
{
    public class Laser : MonoBehaviour, IAttack
    {
        public Type Type => Type.Direct;
        [SerializeField] private Transform laserIndicator;
        [SerializeField] private GameObject laserAttack;
        [SerializeField] private GameObject player;
        [SerializeField] private Player.Controller playerScript;
        [SerializeField] private Transform boss;
        private Vector3 indicatorStartPos = new(20, 0, 0);
        private Vector3 laserStartPos = new(25, 0, 0);
        private float laserLength;
        private float indicatorTime = 1f;
        private int damage = 10;

        private void Awake()
        {
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
            laserIndicator.transform.position = boss.transform.position + laserLength * Vector3.Normalize(player.transform.position - boss.position);
            laserIndicator.transform.LookAt(player.transform);
        }

        private void ShootLaser()
        {
            laserAttack.transform.SetPositionAndRotation(laserIndicator.transform.position, laserIndicator.transform.rotation);
            laserIndicator.transform.position = indicatorStartPos;
        }

        private void ResetLaser()
        {
            laserAttack.transform.position = laserStartPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("player"))
            {
                playerScript.TakeDamage(damage);
            }
        }
    }
}