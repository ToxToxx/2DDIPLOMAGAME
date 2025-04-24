using UnityEngine;

namespace PlayerMovementLogic
{
    public class JumpHandler
    {
        private readonly PlayerMovementModel _model;
        private readonly PlayerMovementController _controller;

        public JumpHandler(PlayerMovementModel model, PlayerMovementController controller)
        {
            _model = model;
            _controller = controller;
        }

        public void JumpChecks()
        {
            if (InputManager.JumpWasPressed)
            {
                if (_model.IsWallSlideFalling && _model.WallJumpPostBufferTimer >= 0f)
                {
                    return;
                }
                else if (_model.IsWallSliding || (_model.IsTouchingWall && _model.IsGrounded)) { return; }

                _model.JumpBufferTimer = _model.MovementStats.JumpBufferTime;
                _model.JumpReleasedDuringBufferTimer = false;
            }

            if (InputManager.JumpWasReleased)
            {
                if (_model.JumpBufferTimer > 0)
                {
                    _model.JumpReleasedDuringBufferTimer = true;
                }

                if (_model.IsJumping && _model.VerticalVelocity > 0f)
                {
                    if (_model.IsPastApexThreshold)
                    {
                        _model.IsPastApexThreshold = false;
                        _model.IsFastFalling = true;
                        _model.FastFallTime = _model.MovementStats.TimeForUpwardsCancel;
                        _model.VerticalVelocity = 0f;
                    }
                    else
                    {
                        _model.IsFastFalling = true;
                        _model.FastFallReleaseSpeed = _model.VerticalVelocity;
                    }
                }
            }

            // Initiating Jump
            if (_model.JumpBufferTimer > 0f && !_model.IsJumping && (_model.IsGrounded || _model.CoyoteTimer > 0f))
            {
                InitiateJump(1);

                if (_model.JumpReleasedDuringBufferTimer)
                {
                    _model.IsFastFalling = true;
                    _model.FastFallReleaseSpeed = _model.VerticalVelocity;
                }
            }

            // Double jump logic
            else if (_model.JumpBufferTimer > 0f && (_model.IsJumping || _model.IsWallJumping || _model.IsWallSlideFalling || _model.IsAirDashing || _model.IsDashFastFalling) && !_model.IsTouchingWall && _model.NumberOfJumpsUsed < _model.MovementStats.NumberOfJumpsAllowed)
            {
                _model.IsFastFalling = false;
                InitiateJump(1);

                if (_model.IsDashFastFalling)
                {
                    _model.IsDashFastFalling = false;
                }
            }

            // Air jump after coyote time lapsed logic
            else if (_model.JumpBufferTimer > 0f && _model.IsFalling && !_model.IsWallSlideFalling && _model.NumberOfJumpsUsed < _model.MovementStats.NumberOfJumpsAllowed - 1)
            {
                InitiateJump(2); // Air jump logic
                _model.IsFastFalling = false;
            }
        }

        private void InitiateJump(int numberOfJumpsUsed)
        {
            if (!_model.IsJumping)
            {
                _model.IsJumping = true;
            }

            _model.ResetWallJumpValues();

            _model.JumpBufferTimer = 0f;
            _model.NumberOfJumpsUsed += numberOfJumpsUsed;
            _model.VerticalVelocity = _model.MovementStats.InitialJumpVelocity;
        }

        public void Jump()
        {
            // Apply gravity
            if (_model.IsJumping)
            {
                // Check head bump
                if (_model.BumpedHead)
                {
                    _model.IsFastFalling = true;
                }

                // Gravity during ascending
                if (_model.VerticalVelocity >= 0f)
                {
                    // Apex controls
                    _model.ApexPoint = Mathf.InverseLerp(_model.MovementStats.InitialJumpVelocity, 0f, _model.VerticalVelocity);

                    if (_model.ApexPoint > _model.MovementStats.ApexThreshhold)
                    {
                        if (!_model.IsPastApexThreshold)
                        {
                            _model.IsPastApexThreshold = true;
                            _model.TimePastApexThreshold = 0f;
                        }

                        if (_model.IsPastApexThreshold)
                        {
                            _model.TimePastApexThreshold += Time.deltaTime;
                            if (_model.TimePastApexThreshold < _model.MovementStats.ApexHangTime)
                            {
                                _model.VerticalVelocity = 0f;
                            }
                            else
                            {
                                _model.VerticalVelocity = -0.01f;
                            }
                        }
                    }

                    // Gravity during ascending not past apex threshold
                    else if (!_model.IsFastFalling)
                    {
                        _model.VerticalVelocity += _model.MovementStats.Gravity * Time.fixedDeltaTime;
                        if (_model.IsPastApexThreshold)
                        {
                            _model.IsPastApexThreshold = false;
                        }
                    }
                }

                // Gravity during descending
                else if (!_model.IsFastFalling)
                {
                    _model.VerticalVelocity += _model.MovementStats.Gravity * _model.MovementStats.GravityReleaseMultiplier * Time.fixedDeltaTime;
                }
                else if (_model.VerticalVelocity < 0f)
                {
                    if (!_model.IsFalling)
                    {
                        _model.IsFalling = true;
                    }
                }
            }

            // Jump cut (Fast falling)
            if (_model.IsFastFalling)
            {
                if (_model.FastFallTime >= _model.MovementStats.TimeForUpwardsCancel)
                {
                    _model.VerticalVelocity += _model.MovementStats.Gravity * _model.MovementStats.GravityReleaseMultiplier * Time.fixedDeltaTime;
                }
                else if (_model.FastFallTime < _model.MovementStats.TimeForUpwardsCancel)
                {
                    _model.VerticalVelocity = Mathf.Lerp(_model.FastFallReleaseSpeed, 0f, (_model.FastFallTime / _model.MovementStats.TimeForUpwardsCancel));
                }
                _model.FastFallTime += Time.fixedDeltaTime;
            }
        }
    }
}
