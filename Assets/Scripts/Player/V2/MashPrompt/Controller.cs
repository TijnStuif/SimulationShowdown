using System;
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
        private bool IsInMashSequence => m_teleportScript.IsInMashSequence;
        
        private void Awake()
        {
            m_teleportScript = GetComponentInParent<Teleport>();
            Root = GetComponent<UIDocument>().rootVisualElement;
            m_f = Root.Q<VisualElement>("F");
            m_j = Root.Q<VisualElement>("J");
        }

        private void OnEnable()
        {
            Teleport.OnBossAttacked += OnTeleportOnBossAttacked;
        }

        private void OnDisable()
        {
            Teleport.OnBossAttacked -= OnTeleportOnBossAttacked;
        }
        
        public void OnMash(InputAction.CallbackContext context)
        {
            if (IsInMashSequence == false) return;
            InputBinding? binding = context.action.GetBindingForControl(context.control);
            if (context.performed)
            {
                if (binding.ToString() == "Mash:<Keyboard>/f[Keyboard&Mouse]")
                    m_f.style.backgroundColor = Color.yellow;
                if (binding.ToString() == "Mash:<Keyboard>/j[Keyboard&Mouse]")
                    m_j.style.backgroundColor = Color.magenta;
            }

            if (context.canceled)
            {
                if (binding.ToString() == "Mash:<Keyboard>/f[Keyboard&Mouse]")
                    m_f.style.backgroundColor = Color.white;
                if (binding.ToString() == "Mash:<Keyboard>/j[Keyboard&Mouse]")
                    m_j.style.backgroundColor = Color.white;
            }
        }

        private void OnTeleportOnBossAttacked(float d, Teleport.MashState key)
        {
            Show();
        }


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // will be refactored, just testing functionality for now
        private void FixedUpdate()
        {
            if (IsInMashSequence == false) Hide();
            
        }
    }
}
