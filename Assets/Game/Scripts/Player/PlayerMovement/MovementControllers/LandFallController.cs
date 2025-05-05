using PlayerEvent;
using UnityEngine;

namespace PlayerMovementLogic
{
    public class LandFallController
    {
        private readonly PlayerMovementModel _model;
        private readonly PlayerMovementController _controller;
        private readonly PlayerEventBus _eventBus;

        public LandFallController(PlayerMovementModel model, PlayerMovementController controller, PlayerEventBus eventBus)
        {
            _model = model;
            _controller = controller;
            _eventBus = eventBus;
        }

        public void LandCheck()
        {
            // Landed logic
            if ((_model.IsJumping || _model.IsFalling || _model.IsWallJumping || _model.IsWallJumpFalling || _model.IsWallSlideFalling || _model.IsWallSliding || _model.IsDashFastFalling) && _model.IsGrounded && _model.VerticalVelocity <= 0f)
            {
                _model.ResetJumpValues();
                _model.StopWallSlide();
                _model.ResetWallJumpValues();
                _model.ResetDashes();
                _model.NumberOfJumpsUsed = 0;
                _model.VerticalVelocity = Physics2D.gravity.y;

                if (_model.IsDashFastFalling && _model.IsGrounded)
                {
                    _model.ResetDashValues();
                    return;
                }

                _model.ResetDashValues();
                _eventBus.RaiseLand();
            }
        }

        public void Fall()
        {
            // Normal gravity while falling
            if (!_model.IsGrounded && !_model.IsJumping && !_model.IsWallSliding && !_model.IsWallJumping && !_model.IsDashing && !_model.IsDashFastFalling)
            {
                if (!_model.IsFalling)
                {
                    _model.IsFalling = true;
                }
                _model.VerticalVelocity += _model.MovementStats.Gravity * Time.fixedDeltaTime;
            }
        }
    }
}
