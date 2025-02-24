using UnityEngine;

namespace Boss.Attack
{
    public class Laser : DamageAttack, IAttack
    {
        public Type Type => Type.Direct;
        private GameObject laserIndicator;
        private GameObject laserAttack;
        private GameObject player;
        private Player.V2.Controller playerScript;
        private GameObject boss;
        //
        
        [SerializeField] private GameObject indicatorPrefab;
        [SerializeField] private GameObject attackPrefab;
        
        private Vector3 indicatorStartPos = new(200, 0, 0);
        private Vector3 laserStartPos = new(250, 0, 0);
        private float laserLength;
        private float indicatorTime = 1f;
        AudioManager audioManager;

        private void Awake()
        {
            // laserIndicator = Instantiate(indicatorPrefab);
            // laserAttack = Instantiate(attackPrefab);
            laserAttack = GameObject.Find("LaserAttack");
            laserIndicator = GameObject.Find("LaserAttackIndicator");
            playerScript = FindObjectOfType<Player.V2.Controller>();
            player = playerScript.gameObject;
            boss = FindObjectOfType<Boss.Controller>().gameObject;
            audioManager = FindObjectOfType<AudioManager>();
            ResetLaser();
            
            laserLength = Vector3.Distance(laserAttack.transform.position, laserAttack.transform.position + laserAttack.transform.localScale / 2);
        }
        // to make script enable and disable'able
        private void Start() {}
    
        // the Execute method is called when the attack is executed
        public void Execute()
        {
            audioManager.PlaySFX(audioManager.bossLaserSFX);
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
    }
}