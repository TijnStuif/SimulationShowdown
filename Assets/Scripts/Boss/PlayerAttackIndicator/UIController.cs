using Player.V2;
using UnityEngine;
using UnityEngine.UIElements;

namespace Boss.PlayerAttackIndicator
{
    public class UiController : AbstractUiController
    {
        private Vector3 m_bossScreenToWorld;
        [SerializeField] private Transform m_bossTransform;
        private Camera m_camera;
        private Teleport.BossRange m_bossRange;
        private ForceField.State m_forceFieldState;

        private ForceField.EventPublisher m_forceFieldEventPublisher;

        private GameObject Boss => m_bossTransform.gameObject;

        private void Awake()
        {
            m_forceFieldEventPublisher = Boss.GetComponentInChildren<ForceField.EventPublisher>();
            m_forceFieldState = ForceField.State.Active;
            m_bossRange = Teleport.BossRange.Outside;
            m_camera = Camera.main;
            Root = GetComponent<UIDocument>().rootVisualElement;
        }

        private void Start() {}

        private void OnEnable()
        {
            Teleport.RangeChange += OnPlayerRangeChange;
            m_forceFieldEventPublisher.StateChanged += OnForceFieldStateChanged;
        }

        private void OnDisable()
        {
            Teleport.RangeChange -= OnPlayerRangeChange;
            m_forceFieldEventPublisher.StateChanged -= OnForceFieldStateChanged;
        }

        private void OnForceFieldStateChanged(ForceField.State state)
        {
            m_forceFieldState = state;
            UpdateUI();
        }

        private void OnPlayerRangeChange(Teleport.BossRange range)
        {
            m_bossRange = range;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (m_forceFieldState == ForceField.State.Inactive && m_bossRange == Teleport.BossRange.Inside)
                Show();
            else
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
