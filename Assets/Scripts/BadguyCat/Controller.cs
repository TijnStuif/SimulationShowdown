using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Timer = System.Timers.Timer;

namespace BadguyCat
{
    public class Controller : MonoBehaviour
    {
        public int Hp => m_hp;

        private bool m_dead;
        
        private int m_hp = 5;
        
        private Timer m_timer;
        
        private NavMeshAgent m_agent;
        private Transform m_player;

        [SerializeField] private GameObject projectilePrefab;

        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsPlayer;

        private Vector3 m_navDestination;
        [SerializeField] private float navDestinationRange;
        [SerializeField] private bool navDestinationBeingSet;

        [SerializeField] private float attackInterval;
        private bool m_attacked;

        [SerializeField] private float sightRadius;
        [SerializeField] private float attackRadius;
        private bool m_inSightRange;
        private bool m_inAttackRange;
        

        private int PlayerLayer => whatIsPlayer;
        private int GroundLayer => whatIsGround;

        private void Start()
        {
            m_agent = GetComponent<NavMeshAgent>();
            m_player = GameObject.Find("PrototypeCat").transform;
            m_navDestination = transform.position;
            m_timer = new Timer(attackInterval);
            m_timer.Elapsed += ResetAttack;
        }

        private void FixedUpdate()
        {
            // circle collision between our given range, and the player
            m_inSightRange = Physics.CheckSphere(transform.position, sightRadius, PlayerLayer);
            m_inAttackRange = Physics.CheckSphere(transform.position, attackRadius, PlayerLayer);

            if (!m_inSightRange)
            {
                PlayerUndetected();
                return;
            }

            if (!m_inAttackRange)
            {
                PlayerMoveTowards();
                return;
            }

            PlayerAttack();
        }
        
        private void OnDisable()
        {
            if (!m_dead)
                return;
            // drop pickup
            var obj = ItemManager.Instance.GetRandomItemDrop();
            obj.transform.position = transform.position;
            obj.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerHitbox"))
            {
                // I think this ideally gets handled by events, but this probably works too
                var controller = other.gameObject.GetComponent<PrototypeCat.Projectile.Controller>();
                Hit(controller.damage);
            }
            
        }

        public void Hit(int damage)
        {
            m_hp -= damage;
            if (m_hp < 1)
            {
                m_dead = true;
                Destroy(this);
                Destroy(gameObject);    
            }
        }

        private bool SetNavDestination()
        {
            // keep trying to set a nav destination until it is on the ground


            // get two random offsets 
            float randomX = Random.Range(-navDestinationRange, navDestinationRange);
            float randomZ = Random.Range(-navDestinationRange, navDestinationRange);
            // use for new nav destination
            m_navDestination = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);
            // if nav destination is invalid, return false, this would repeat the method at the next frame
            // I think it would be cool if it could map out what is not the ground if it's called too many times, but
            // no time for that
            return (Physics.Raycast(m_navDestination, -transform.up, 2f, GroundLayer)) ;

        }

        private void ResetNavDestination()
        {
            m_navDestination = transform.position;
        }

        private void PlayerUndetected()
        {
            if (m_navDestination == transform.position || navDestinationBeingSet)
            {
                navDestinationBeingSet = true;
                if (!SetNavDestination())
                {
                    m_navDestination = transform.position;
                    return;
                }
                m_agent.SetDestination(m_navDestination);
                navDestinationBeingSet = false;
                return;
            }

            var distance = transform.position - m_navDestination;
            if (distance.magnitude < 1f)
            {
                // stand still and find another nav destination
                ResetNavDestination();
                m_agent.SetDestination(m_navDestination);
            }
        }

        private void PlayerMoveTowards() => m_agent.SetDestination(m_player.position);

        private void PlayerAttack()
        {
            // I thought maybe I could rewrite this for fun, but it calls an extern, so it's probably done in C++
            transform.LookAt(m_player);
            m_agent.SetDestination(transform.position);
            
            if (m_attacked) return;
            
            // m_agent.SetDestination(transform.position);
            
            // attack code here
            m_attacked = true;
            
            // this looks like literally the coolest thing ever,
            // but it uses reflection so it's very expensive
        }

        private void ResetAttack(object sender, ElapsedEventArgs args)
        {
            Debug.Log("stopping attack");
            m_attacked = false;
            // reset attack code here
        }
    }
}
