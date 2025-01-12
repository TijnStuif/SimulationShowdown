using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR;

namespace Player.V2
{
    public class Teleport : MonoBehaviour
    {
        public enum MashState
        {
           Start,
           End,
        }
        public enum BossRange
        {
           Inside,
           Outside
        }
        public readonly float Cooldown = 2f;
        public bool IsInMashSequence => m_isInMashSequence;
        
        private const float AIM_THRESHOLD = 50f;
        private const float ATTACK_RANGE = 10f;
        /// <summary>
        /// scalar for moving the player slightly away from the boss when attacking
        /// </summary>
        private const float BOSS_ATTACK_OFFSET = 2f;

        private const float MASH_LENGTH = 3f;
        private const float MAX_TELEPORT_DISTANCE = 20f;
        private const float TELEPORT_COOLDOWN = 2f;
        
        public static event Action<BossRange> RangeChange;
        public static event Action<MashState> MashSequenceStateChange;
        public static event Action<float> OnBossAttacked;
        
        [SerializeField] private Movement m_movement;
        [SerializeField] private float m_damage = 0.3f;
        
        private AudioManager m_audioManager;
        
        private Transform m_bossTransform;

        private bool m_isInMashSequence;
        private Camera m_mainCamera;

        private float m_timeSinceLastTeleport;
        
        private bool m_inAttackRange;
        
        private Vector3 m_directionToBoss;

        private Vector3 m_direction3d;
        
        public event Action Teleported;
        
        private bool BossAttacked
        {
            get
            {
                UpdateAttackRange();
                if (!InAttackRange) return false;
                    
                #if DEBUG
                Debug.Log("Attacking Boss!");
                #endif
                
                StartCoroutine(ButtonMashSequence());
                // button mash period of time
                // every input does a lil bit of damage
                
                // unfreeze player and boss
                return true;
            }
        }
        
        private bool InAttackRange
        {
            get => m_inAttackRange;
            set
            {
                if (InAttackRange == value)
                    throw new InvalidOperationException("ERROR: Please do not adjust this value carelessly, " +
                                                        "it invokes an event\n"+
                                                        "please only call this setter if the new value is different");
                switch (value)
                {
                    case true:
                        #if DEBUG
                        Debug.Log("in range!");
                        #endif
                        RangeChange?.Invoke(BossRange.Inside);
                        break;
                    case false:
                        #if DEBUG
                        Debug.Log("outside range!");
                        #endif
                        RangeChange?.Invoke(BossRange.Outside);
                        break;
                }

                m_inAttackRange = value;
            }
        }

        private bool CooldownInactive => ((Time.time - m_timeSinceLastTeleport) >= TELEPORT_COOLDOWN);
        

        private void Awake()
        {
            // My solution to avoid GameObject.Find
            // is finding a unique component of the object I need
            // example: Boss.Controller class is only used for the Boss GameObject
            m_bossTransform = FindObjectOfType<Boss.Controller>().transform;
            m_audioManager = FindObjectOfType<AudioManager>();
            m_mainCamera = Camera.main;
        }
        
        private void Update()
        {
            UpdateAttackRange();
            // #if DEBUG
            // Debug.Log(Vector3.Distance(transform.position, m_bossTransform.position));
            // #endif
        }

        private IEnumerator ButtonMashSequence()
        {
            // fine
            MashSequenceStateChange?.Invoke(MashState.Start);
            m_isInMashSequence = true;
            
            float distanceToBoss = Vector3.Distance(transform.position, m_bossTransform.position);
            UpdateDirectionToBoss();
            // teleport in front of boss
            transform.position += m_directionToBoss * distanceToBoss;
            transform.position -= new Vector3(m_directionToBoss.x, 0, m_directionToBoss.z) * BOSS_ATTACK_OFFSET;
                
            // force player to face boss
            var angle = Mathf.Atan2(
                            y: m_bossTransform.position.x - transform.position.x, 
                            x: m_bossTransform.position.z - transform.position.z) 
                            * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
            
            yield return new WaitForSeconds(MASH_LENGTH);
            
            MashSequenceStateChange?.Invoke(MashState.End);
            m_isInMashSequence = false;
            
        }
        
        public void OnMash(InputAction.CallbackContext context)
        {
            if (m_isInMashSequence == false) return;
                OnBossAttacked?.Invoke(m_damage);
        }
        
        private void OnTeleport(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!CooldownInactive) return;
                
                m_timeSinceLastTeleport = Time.time;
                // transform.velocity = Vector3.zero;

                m_direction3d = m_movement.FullMoveDirection3d;

                if (BossAttacked)
                {
                    m_audioManager.PlaySFX(m_audioManager.playerTeleportedSFX);
                    m_audioManager.PlaySFX(m_audioManager.bossDamagedSFX[UnityEngine.Random.Range(0, m_audioManager.bossDamagedSFX.Length)]);
                    Teleported?.Invoke();
                    return;
                }

                #if DEBUG
                Debug.Log("Teleporting!");
                #endif
                
                StandardTeleport();
                Teleported?.Invoke();
            }
        }
        
        private void StandardTeleport()
        {
                m_movement.FreezeController();
                // check if teleporting in wall
                if (Physics.Raycast(
                        origin: m_movement.transform.position, 
                        direction: m_movement.FullMoveDirection3d, 
                        hitInfo: out RaycastHit hit, 
                        maxDistance: MAX_TELEPORT_DISTANCE))
                {
                    // teleport right in front of the wall
                    m_movement.transform.position = hit.point - m_movement.FullMoveDirection3d * 0.5f; 
                }
                else
                {
                    // teleport max distance
                    transform.position += m_movement.FullMoveDirection3d * MAX_TELEPORT_DISTANCE;
                }
                m_movement.ThawController();
                m_audioManager.PlaySFX(m_audioManager.playerTeleportedSFX);
                Teleported?.Invoke();
        }

        private void UpdateAttackRange()
        {
            // float angleToBoss = Vector3.Angle(m_movement.FullMoveDirection3d, m_bossTransform.position);
            float bossDistance = Vector3.Distance(m_bossTransform.position, transform.position);

            // bool inAttackRange = (angleToBoss <= AIM_THRESHOLD && bossDistance <= ATTACK_RANGE);
            // in this case player direction doesn't matter and it's just attack range
            bool inAttackRange = (bossDistance <= ATTACK_RANGE);
            if (InAttackRange == inAttackRange) 
                return;
            InAttackRange = inAttackRange;
        }

        private void UpdateDirectionToBoss()
        {
            m_directionToBoss = (m_bossTransform.position - transform.position).normalized;
        }

    }
}
