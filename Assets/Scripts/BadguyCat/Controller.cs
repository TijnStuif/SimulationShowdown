using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BadguyCat
{
    public class Controller : MonoBehaviour
    {
        private Timer m_timer;
        
        private NavMeshAgent m_agent;
        private Transform m_player;

        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsPlayer;

        private Vector3 m_walkPoint;
        [SerializeField] private float walkPointRange;
        [SerializeField] private bool walkPointBeingSet;

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
            m_walkPoint = transform.position;
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

        private bool SetWalkPoint()
        {
            // keep trying to set a walk point until it is on the ground


            // get two random offsets 
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            // use for new walk point
            m_walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);
            // if walk point invalid, return false, this would repeat the method at the next frame
            // I think it would be cool if it could map out what is not the ground if it's called too many times, but
            // no time for that
            return (Physics.Raycast(m_walkPoint, -transform.up, 2f, GroundLayer)) ;

        }

        private void ResetWalkPoint()
        {
            m_walkPoint = transform.position;
        }

        private void PlayerUndetected()
        {
            if (m_walkPoint == transform.position || walkPointBeingSet)
            {
                walkPointBeingSet = true;
                if (!SetWalkPoint())
                {
                    m_walkPoint = transform.position;
                    return;
                }
                m_agent.SetDestination(m_walkPoint);
                walkPointBeingSet = false;
                return;
            }

            var distance = transform.position - m_walkPoint;
            if (distance.magnitude < 1f)
            {
                // stand still and find another walk point
                ResetWalkPoint();
                m_agent.SetDestination(m_walkPoint);
            }
        }

        private void PlayerMoveTowards() => m_agent.SetDestination(m_player.position);

        private void PlayerAttack()
        {
            // I thought maybe I could rewrite this for fun, but it calls an extern, so it's probably done in C++
            transform.LookAt(m_player);
            
            if (m_attacked) return;
            
            // m_agent.SetDestination(transform.position);
            
            // attack code here
            Debug.Log("attacking!!!111");
            m_attacked = true;
            //
            
            
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
