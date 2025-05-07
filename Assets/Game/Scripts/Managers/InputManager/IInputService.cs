using UnityEngine;

namespace InGameInput
{
    public interface IInputService
    {
        Vector2 Movement { get; }
        bool JumpWasPressed { get; }
        bool JumpIsHeld { get; }
        bool JumpWasReleased { get; }

        bool RunIsHeld { get; }
        bool DashWasPressed { get; }

        bool InteractionWasPressed { get; }
        bool AttackWasPressed { get; }
    }
}

