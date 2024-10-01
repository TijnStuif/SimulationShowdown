using UnityEngine;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PrototypeCat
{
    public class Slash : MonoBehaviour
    {
        [SerializeField] private GameObject attackPrefab;
        private GameObject m_attack;
        private Timer m_cooldownTimer;
        private bool m_attackOk = true;

        private void Awake()
        {
            m_cooldownTimer = new Timer(1000f);
            m_attack = Instantiate(attackPrefab, transform, false);
            m_attack.SetActive(false);
        }

        private void OnEnable()
        {
            m_cooldownTimer.Elapsed += ResetSlash;
        }

        private void OnDisable()
        {
            m_cooldownTimer.Elapsed -= ResetSlash;
        }

        // activates?
        // break points don't work on this entire file though? ???
        private void OnSlash()
        {
            // activates
            Debug.Log("slashing ! ! ");
            if (!m_attackOk) return;
            m_attackOk = false;
            m_attack.SetActive(true);
            m_cooldownTimer.Start();
        }

        private void ResetSlash(object sender, ElapsedEventArgs e)
        {
            // activates
            Debug.Log("swish over");
            // doesn't work?
            m_attack.SetActive(false);
            // doesn't work (?)
            m_attackOk = true;
            // doesn't stop??
            m_cooldownTimer.Stop();
        }
    }
}
