using UnityEngine;

namespace PlayerMovementLogic
{
    public class TimerController
    {
        private readonly PlayerMovementModel _model;
        private readonly WallJumpController _wallJumpController;

        public TimerController(PlayerMovementModel model, WallJumpController wallJumpController)
        {
            _model = model;
            _wallJumpController = wallJumpController;
        }

        public void CountTimers()
        {
            _model.JumpBufferTimer -= Time.deltaTime;

            if (!_model.IsGrounded)
            {
                _model.CoyoteTimer -= Time.deltaTime;
            }
            else
            {
                _model.CoyoteTimer = _model.MovementStats.JumpCoyoteTime;
            }

            if (!_wallJumpController.ShouldApplyPostWallJumpBuffer())
            {
                _model.WallJumpPostBufferTimer -= Time.deltaTime;
            }

            if (_model.IsGrounded)
            {
                _model.DashOnGroundTimer -= Time.deltaTime;
            }
        }
    }
}
