using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerAttackIndicator
{
    public class Controller : AbstractUiController
    {
        private Transform m_bossTransform;
        private Camera m_camera;
        private VisualElement m_indicator;
        private bool m_shouldUpdate;
        private Vector3 m_screenPosition;
        
        private Vector3 BossPositionToScreenPoint => m_camera.ScreenToWorldPoint(m_bossTransform.position);
        private void Awake()
        {
            m_camera = Camera.main;
            Root = GetComponent<UIDocument>().rootVisualElement;
            m_indicator = Root.Q<VisualElement>("indicator");
            m_bossTransform = FindObjectOfType<Boss.Controller>().transform;
        }

        private void Start() {}

        private void OnEnable()
        {
            Player.V2.Teleport.RangeChange += OnPlayerRangeChange;
        }

        private void OnDisable()
        {
            Player.V2.Teleport.RangeChange -= OnPlayerRangeChange;
        }

        private void OnPlayerRangeChange(Player.V2.Teleport.BossRange range)
        {
            if (range == Player.V2.Teleport.BossRange.Inside)
            {
                Show();
                m_shouldUpdate = true;
                return;
            }
            Hide();
            m_shouldUpdate = false;
        }

        private void FixedUpdate()
        {
            if (m_shouldUpdate == false)
                return;
            m_screenPosition = 
            m_camera.WorldToScreenPoint(m_bossTransform.position);
            m_screenPosition.y -= Screen.height / 2f;
            m_screenPosition.z = 0;
            Root.transform.position = m_screenPosition;
        }
    }
}
