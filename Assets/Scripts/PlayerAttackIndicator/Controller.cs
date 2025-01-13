using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerAttackIndicator
{
    public class Controller : AbstractUiController
    {
        private Vector3 m_bossScreenToWorld;
        private Transform m_bossTransform;
        private Camera m_camera;

        private void Awake()
        {
            m_camera = Camera.main;
            Root = GetComponent<UIDocument>().rootVisualElement;
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
                return;
            }
            Hide();
        }

        private void Update()
        {
            var bossScreenPoint = m_camera.WorldToScreenPoint(m_bossTransform.position);
            m_bossScreenToWorld.Set(
                newX: bossScreenPoint.x,
                // un-flip vertical screen position
                newY: Screen.height - bossScreenPoint.y,
                newZ: 0);
            // update UI element position
            Root.transform.position = m_bossScreenToWorld;
        }
    }
}
