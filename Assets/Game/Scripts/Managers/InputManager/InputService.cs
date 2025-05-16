using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InGameInput
{
    public class InputService : IInitializable, ITickable, IDisposable, IInputService
    {
        public Vector2 Movement { get; private set; }
        public bool JumpWasPressed { get; private set; }
        public bool JumpIsHeld { get; private set; }
        public bool JumpWasReleased { get; private set; }
        public bool RunIsHeld { get; private set; }
        public bool DashWasPressed { get; private set; }
        public bool InteractionWasPressed { get; private set; }
        public bool AttackWasPressed { get; private set; }

        private readonly PlayerInput _playerInput;

        private InputAction _move;
        private InputAction _jump;
        private InputAction _run;
        private InputAction _dash;
        private InputAction _interact;
        private InputAction _attack;

        public InputService(PlayerInput playerInput) => _playerInput = playerInput;

        public void Initialize()
        {
            _move = _playerInput.actions["Move"];
            _jump = _playerInput.actions["Jump"];
            _run = _playerInput.actions["Run"];
            _dash = _playerInput.actions["Dash"];
            _interact = _playerInput.actions["Interact"];
            _attack = _playerInput.actions["Attack"];
        }

        public void Tick()
        {
            Movement = _move.ReadValue<Vector2>();
            JumpWasPressed = _jump.WasPressedThisFrame();
            JumpIsHeld = _jump.IsPressed();
            JumpWasReleased = _jump.WasReleasedThisFrame();
            RunIsHeld = _run.IsPressed();
            DashWasPressed = _dash.WasPressedThisFrame();
            InteractionWasPressed = _interact.WasPressedThisFrame();
            AttackWasPressed = _attack.WasPressedThisFrame();
        }

        public void Dispose() { /* ничего освобождать не нужно */ }
    }

}
