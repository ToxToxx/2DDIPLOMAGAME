using UnityEngine;

namespace PlayerMovementRefactoring
{
    public class TimerController 
    {
        private readonly PlayerMovement _playerMovement;
        private readonly WallJumpController _wallJumpController;

        public TimerController(PlayerMovement player, WallJumpController wallJumpController)
        {
            _playerMovement = player;
            _wallJumpController = wallJumpController;
        }

        public void CountTimers()
        {
            _playerMovement.JumpBufferTimer -= Time.deltaTime;

            if (!_playerMovement.IsGrounded)
            {
                _playerMovement.CoyoteTimer -= Time.deltaTime;
            }
            else
            {
                _playerMovement.CoyoteTimer = _playerMovement.MovementStats.JumpCoyoteTime;
            }

            if (!_wallJumpController.ShouldApplyPostWallJumpBuffer())
            {
                _playerMovement.WallJumpPostBufferTimer -= Time.deltaTime;
            }

            if (_playerMovement.IsGrounded)
            {
                _playerMovement.DashOnGroundTimer -= Time.deltaTime;
            }
        }
    }
}
 
