using Unity.VisualScripting;
using UnityEngine;

namespace PlayerMovementLogic
{
    public class DashController
    {
        private readonly PlayerMovementModel _model;
        private readonly PlayerMovementController _controller;
        private readonly PlayerEventBus _eventBus;

        public DashController(PlayerMovementModel model, PlayerMovementController controller, PlayerEventBus eventBus)
        {
            _model = model;
            _controller = controller;
            _eventBus = eventBus;
        }

        public void DashCheck()
        {
            if (InputManager.DashWasPressed)
            {
                // Ground dash
                if (_model.IsGrounded && _model.DashOnGroundTimer < 0 && !_model.IsDashing)
                {
                    InitiateDash();
                }

                // Air dash
                else if (!_model.IsGrounded && !_model.IsDashing && _model.NumberOfDashesUsed < _model.MovementStats.NumberOfDashes)
                {
                    _model.IsAirDashing = true;
                    InitiateDash();

                    // You left a wallslide but dashed within the wall jump post buffer timer
                    if (_model.WallJumpPostBufferTimer > 0f)
                    {
                        _model.NumberOfJumpsUsed--;
                        if (_model.NumberOfJumpsUsed < 0f)
                        {
                            _model.NumberOfJumpsUsed = 0;
                        }
                    }
                }
            }
        }

        private void InitiateDash()
        {
            _model.DashDirection = InputManager.Movement;

            Vector2 closestDirection = Vector2.zero;
            float minDistance = Vector2.Distance(_model.DashDirection, _model.MovementStats.DashDirections[0]);

            for (int i = 0; i < _model.MovementStats.DashDirections.Length; i++)
            {
                // Skip if we hit it bang on
                if (_model.DashDirection == _model.MovementStats.DashDirections[i])
                {
                    closestDirection = _model.DashDirection;
                    break;
                }

                float distance = Vector2.Distance(_model.DashDirection, _model.MovementStats.DashDirections[i]);

                // If diagonal direction
                bool isDiagonal = (Mathf.Abs(_model.MovementStats.DashDirections[i].x) == 1 && Mathf.Abs(_model.MovementStats.DashDirections[i].y) == 1);
                if (isDiagonal)
                {
                    distance -= _model.MovementStats.DashDiagonallyBias;
                }
                else if (distance < minDistance)
                {
                    minDistance = distance;
                    closestDirection = _model.MovementStats.DashDirections[i];
                }
            }

            // Handle direction with no input
            if (closestDirection == Vector2.zero)
            {
                if (_model.IsFacingRight)
                {
                    closestDirection = Vector2.right;
                }
                else closestDirection = Vector2.left;
            }

            _model.DashDirection = closestDirection;
            _model.NumberOfDashesUsed++;
            _model.IsDashing = true;
            _model.DashTimer = 0f;
            _model.DashOnGroundTimer = _model.MovementStats.TimeBtwDashesOnGround;

            _model.ResetJumpValues();
            _model.ResetWallJumpValues();
            _model.StopWallSlide();
        }

        public void Dash()
        {
           
            if (_model.IsDashing)
            {
                // Stop the dash after the timer
                _model.DashTimer += Time.fixedDeltaTime;
                if (_model.DashTimer >= _model.MovementStats.DashTime)
                {
                    if (_model.IsGrounded)
                    {
                        _model.ResetDashes();
                    }

                    _model.IsAirDashing = false;
                    _model.IsDashing = false;

                    if (!_model.IsJumping && !_model.IsWallJumping)
                    {
                        _model.DashFastFallTime = 0f;
                        _eventBus.RaiseDash();
                        _model.DashFastFallReleaseSpeed = _model.VerticalVelocity;

                        if (!_model.IsGrounded)
                        {
                            _model.IsDashFastFalling = true;
                        }
                    }

                    return;
                }

                _model.HorizontalVelocity = _model.MovementStats.DashSpeed * _model.DashDirection.x;

                if (_model.DashDirection.y != 0f || _model.IsAirDashing)
                {
                    _model.VerticalVelocity = _model.MovementStats.DashSpeed * _model.DashDirection.y;
                }
            }

            // Handle dash cut time
            else if (_model.IsDashFastFalling)
            {
                if (_model.VerticalVelocity > 0f)
                {
                    if (_model.DashFastFallTime < _model.MovementStats.DashTimeForUpwardsCancel)
                    {
                        _model.VerticalVelocity = Mathf.Lerp(_model.DashFastFallReleaseSpeed, 0f, (_model.DashFastFallTime / _model.MovementStats.DashTimeForUpwardsCancel));
                    }
                    else if (_model.DashFastFallTime >= _model.MovementStats.DashTimeForUpwardsCancel)
                    {
                        _model.VerticalVelocity += _model.MovementStats.Gravity * _model.MovementStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                    }

                    _model.DashFastFallTime += Time.fixedDeltaTime;
                }
                else
                {
                    _model.VerticalVelocity += _model.MovementStats.Gravity * _model.MovementStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
            }
        }
    }
}
