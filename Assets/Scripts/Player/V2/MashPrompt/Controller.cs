using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Player.V2.MashPrompt
{
    public class Controller : AbstractUiController
    {
        private VisualElement m_f;
        private VisualElement m_j;
        private Teleport m_teleportScript;
        private bool m_isInMashSequence;
        
        private void Awake()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            Hide();
            m_f = Root.Q<VisualElement>("F");
            m_j = Root.Q<VisualElement>("J");
        }

        private void OnEnable()
        {
            Teleport.MashSequenceStateChange += OnMashSequenceStateChange;
        }

        private void OnDisable()
        {
            Teleport.MashSequenceStateChange -= OnMashSequenceStateChange;
        }

        private void OnMashSequenceStateChange(Teleport.MashState state)
        {
            ResetKeyColors();
            switch (state)
            {
                case Teleport.MashState.Start:
                    m_isInMashSequence = true;
                    Show();
                    break;
                case Teleport.MashState.End:
                    Hide();
                    m_isInMashSequence = false;
                    break;
            }
        }

        public void OnMash(InputAction.CallbackContext context)
        {
            if (m_isInMashSequence == false) return;
            InputBinding? binding = context.action.GetBindingForControl(context.control);
            if (context.performed)
            {
                // isn't able to register both when both are pressed ):
                if (binding == context.action.bindings[0])
                    m_f.style.backgroundColor = Color.yellow;
                if (binding == context.action.bindings[1])
                    m_j.style.backgroundColor = Color.magenta;
            }

            if (context.canceled)
            {
                if (binding == context.action.bindings[0])
                    m_f.style.backgroundColor = Color.black;
                if (binding == context.action.bindings[1])
                    m_j.style.backgroundColor = Color.black;
            }
        }

        private void ResetKeyColors()
        {
            m_f.style.backgroundColor = Color.black;
            m_j.style.backgroundColor = Color.black;
        }


        // Start is called before the first frame update
        void Start() {}
    }
}
