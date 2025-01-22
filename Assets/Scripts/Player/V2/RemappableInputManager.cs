using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RemappableInputManager : MonoBehaviour
{
    // could use a dictionary
    public enum PlayerAction
    {
       Move,
       Jump,
       Pause,
       Teleport,
       MashKey
    }
    Dictionary<PlayerAction, InputAction> PlayerActions =
        new ()
        {
            {PlayerAction.Move, new InputAction() },
            {PlayerAction.Jump, new InputAction() },
            {PlayerAction.Pause, new InputAction() },
            {PlayerAction.Teleport, new InputAction() },
            {PlayerAction.MashKey, new InputAction() }
        };
    
    private PlayerInput m_input;
    
    private InputAction m_moveAction;
    private InputAction m_jumpAction;
    private InputAction m_pauseAction;
    private InputAction m_teleportAction;
    private InputAction m_mashAction;

    public void Remap(PlayerAction action, int index)
    {
        var input = PlayerActions[action];
        input.PerformInteractiveRebinding(index)
            .WithCancelingThrough("<keyboard>/escape")
            .WithControlsExcluding("<mouse>");
    }

    private void Awake()
    {
        m_input = GetComponent<PlayerInput>();
        if (m_input == null) throw new NullReferenceException("damn.");
        InitInputs();
    }

    // private void Update()
    // {
    //     GetRemappableInputs();
    // }

    private void InitInputs()
    {
       PlayerActions[PlayerAction.Move] = m_input.actions["Move"];
       PlayerActions[PlayerAction.Jump] = m_input.actions["Jump"];
       PlayerActions[PlayerAction.Pause] = m_input.actions["Pause"];
       PlayerActions[PlayerAction.Teleport] = m_input.actions["Teleport"];
       PlayerActions[PlayerAction.MashKey] = m_input.actions["Mash"];
    }

    private void GetRemappableInputs()
    {
       // Move = m_moveAction.ReadValue<Vector2>();
       // Jump = m_jumpAction.WasPressedThisFrame();
       // Pause = m_pauseAction.WasPressedThisFrame();
       // Teleport = m_teleportAction.WasPressedThisFrame();
       // MashKey = m_mashAction.ReadValue<float>();
    }
}
