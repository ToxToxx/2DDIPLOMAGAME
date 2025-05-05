using UnityEngine;

namespace PlayerMovementLogic
{
    public class WallJumpController
    {
        private readonly PlayerMovementModel _model;
        private readonly PlayerMovementController _controller;
        private readonly Transform _transform;

        public WallJumpController(PlayerMovementModel model, PlayerMovementController controller, Transform transform)
        {
            _model = model;
            _controller = controller;
            _transform = transform;
        }

        public void WallJumpCheck()
        {
            if (ShouldApplyPostWallJumpBuffer())
            {
                _model.WallJumpPostBufferTimer = _model.MovementStats.WallJumpPostBufferTime;
            }

            // wall jump fast falling
            if (InputManager.JumpWasReleased && !_model.IsWallSliding && !_model.IsTouchingWall && _model.IsWallJumping)
            {
                if (_model.VerticalVelocity > 0f)
                {
                    if (_model.IsPastWallJumpApexThreshold)
                    {
                        _model.IsPastWallJumpApexThreshold = false;
                        _model.IsWallJumpFastFalling = true;
                        _model.WallJumpFastFallTime = _model.MovementStats.TimeForUpwardsCancel;

                        _model.VerticalVelocity = 0f;
                    }
                    else
                    {
                        _model.IsWallJumpFastFalling = true;
                        _model.WallJumpFastFallReleaseSpeed = _model.VerticalVelocity;
                    }
                }
            }

            // actual jump with post wall jump buffer time
            if (InputManager.JumpWasPressed && _model.WallJumpPostBufferTimer > 0f)
            {
                InitialWallJump();
            }
        }

        private void InitialWallJump()
        {
            if (!_model.IsWallJumping)
            {
                _model.IsWallJumping = true;
                _model.UseWallJumpMoveStats = true;
            }

            _model.StopWallSlide(); // Calling stop through controller
            _model.ResetJumpValues();
            _model.WallJumpTime = 0f;

            _model.VerticalVelocity = _model.MovementStats.InitialWallJumpVelocity;

            int dirMultiplier = 0;
            Vector2 hitPoint = _model.LastWallHit.collider.ClosestPoint(_model.BodyCollider.bounds.center);

            if (hitPoint.x > _transform.position.x)
            {
                dirMultiplier = -1;
            }
            else
            {
                dirMultiplier = 1;
            }

            _model.HorizontalVelocity = Mathf.Abs(_model.MovementStats.WallJumpDirection.x) * dirMultiplier;
        }

        public void WallJump()
        {
            // Apply Wall Jump Gravity
            if (_model.IsWallJumping)
            {
                _model.WallJumpTime += Time.fixedDeltaTime;
                if (_model.WallJumpTime >= _model.MovementStats.TimeTillJumpApex)
                {
                    _model.UseWallJumpMoveStats = false;
                }

                // Hit head
                if (_model.BumpedHead)
                {
                    _model.IsWallJumpFastFalling = true;
                    _model.UseWallJumpMoveStats = false;
                }

                // Gravity in Ascending
                if (_model.VerticalVelocity >= 0f)
                {
                    // Apex controls
                    _model.WallJumpApexPoint = Mathf.InverseLerp(_model.MovementStats.WallJumpDirection.y, 0f, _model.VerticalVelocity);

                    if (_model.WallJumpApexPoint > _model.MovementStats.ApexThreshhold)
                    {
                        if (!_model.IsPastWallJumpApexThreshold)
                        {
                            _model.IsPastWallJumpApexThreshold = true;
                            _model.TimePastWallJumpApexThreshold = 0f;
                        }
                        if (_model.IsPastWallJumpApexThreshold)
                        {
                            _model.TimePastWallJumpApexThreshold += Time.fixedDeltaTime;
                            if (_model.TimePastWallJumpApexThreshold < _model.MovementStats.ApexHangTime)
                            {
                                _model.VerticalVelocity = 0f;
                            }
                            else
                            {
                                _model.VerticalVelocity = -0.01f;
                            }
                        }
                    }
                    // Gravity in ascending but not past apex threshold
                    else if (!_model.IsWallJumpFastFalling)
                    {
                        _model.VerticalVelocity += _model.MovementStats.WallJumpGravity * Time.fixedDeltaTime;

                        if (_model.IsPastWallJumpApexThreshold)
                        {
                            _model.IsPastWallJumpApexThreshold = false;
                        }
                    }
                }
                // Gravity in Descending
                else if (!_model.IsWallJumpFastFalling)
                {
                    _model.VerticalVelocity += _model.MovementStats.WallJumpGravity * Time.fixedDeltaTime;
                }

                else if (_model.VerticalVelocity < 0f)
                {
                    if (!_model.IsWallJumpFalling)
                        _model.IsWallJumpFalling = true;
                }
            }

            // Handle wall jump cut time
            if (_model.IsWallJumpFastFalling)
            {
                if (_model.WallJumpFastFallTime >= _model.MovementStats.TimeForUpwardsCancel)
                {
                    _model.VerticalVelocity += _model.MovementStats.WallJumpGravity * _model.MovementStats.WallJumpGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
                else if (_model.WallJumpFastFallTime < _model.MovementStats.TimeForUpwardsCancel)
                {
                    _model.VerticalVelocity = Mathf.Lerp(_model.WallJumpFastFallReleaseSpeed, 0f, (_model.WallJumpFastFallTime / _model.MovementStats.TimeForUpwardsCancel));
                }

                _model.WallJumpFastFallTime += Time.fixedDeltaTime;
            }
        }

        public bool ShouldApplyPostWallJumpBuffer()
        {
            if (!_model.IsGrounded && (_model.IsTouchingWall || _model.IsWallSliding))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}