using PlayerEvent;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerMovement
{
    public class WallSlideController
    {
        private readonly PlayerMovementModel _model;
        private readonly PlayerMovementController _controller;
        private readonly PlayerEventBus _eventBus;

        public WallSlideController(PlayerMovementModel model, PlayerMovementController controller, PlayerEventBus eventBus)
        {
            _model = model;
            _controller = controller;
            _eventBus = eventBus;
        }

        public void WallSlideCheck()
        {
            if (_model.IsTouchingWall && !_model.IsGrounded && !_model.IsDashing)
            {
                if (_model.VerticalVelocity < 0f && !_model.IsWallSliding)
                {
                    _eventBus.RaiseWallSlideStart();
                    _model.ResetJumpValues();
                    _model.ResetWallJumpValues();
                    _model.ResetDashValues();

                    if (_model.MovementStats.ResetDashOnWallSlide)
                    {
                        _model.ResetDashes();
                    }

                    _model.IsWallSlideFalling = false;
                    _model.IsWallSliding = true;

                    if (_model.MovementStats.ResetJumpOnWallSlide)
                    {
                        _model.NumberOfJumpsUsed = 0;
                    }
                }
            }
            else if (_model.IsWallSliding && !_model.IsTouchingWall && !_model.IsGrounded && !_model.IsWallSlideFalling)
            {
                _model.IsWallSlideFalling = true;
                _model.StopWallSlide();
            }
            else
            {
                _model.StopWallSlide();
            }
        }

        public void WallSlide()
        {
            if (_model.IsWallSliding)
            {
                
                _model.VerticalVelocity = Mathf.Lerp(_model.VerticalVelocity, -_model.MovementStats.WallSlideSpeed, _model.MovementStats.WallSlideDecelerationSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
