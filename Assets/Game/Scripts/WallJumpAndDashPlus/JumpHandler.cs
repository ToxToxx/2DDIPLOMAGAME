using UnityEngine;

namespace PlayerMovementRefactoring
{
    public class JumpHandler
    {
        private readonly PlayerMovement _playerMovement;

        public JumpHandler(PlayerMovement player)
        {
            _playerMovement = player;
        }

        public void JumpChecks()
        {
            if (InputManager.JumpWasPressed)
            {
                if (_playerMovement.IsWallSlideFalling && _playerMovement.WallJumpPostBufferTimer >= 0f)
                {
                    return;
                }

                else if (_playerMovement.IsWallSliding || (_playerMovement.IsTouchingWall && _playerMovement.IsGrounded)) { return; }

                _playerMovement.JumpBufferTimer = _playerMovement.MovementStats.JumpBufferTime;
                _playerMovement.JumpReleasedDuringBufferTimer = false;
            }

            if (InputManager.JumpWasReleased)
            {
                if (_playerMovement.JumpBufferTimer > 0)
                {
                    _playerMovement.JumpReleasedDuringBufferTimer = true;
                }

                if (_playerMovement.IsJumping && _playerMovement.VerticalVelocity > 0f)
                {
                    if (_playerMovement.IsPastApexThreshold)
                    {
                        _playerMovement.IsPastApexThreshold = false;
                        _playerMovement.IsFastFalling = true;
                        _playerMovement.FastFallTime = _playerMovement.MovementStats.TimeForUpwardsCancel;
                        _playerMovement.VerticalVelocity = 0f;
                    }
                    else
                    {
                        _playerMovement.IsFastFalling = true;
                        _playerMovement.FastFallReleaseSpeed = _playerMovement.VerticalVelocity;
                    }
                }
            }

            //Initiating Jump
            if (_playerMovement.JumpBufferTimer > 0f && !_playerMovement.IsJumping && (_playerMovement.IsGrounded || _playerMovement.CoyoteTimer > 0f))
            {
                InitiateJump(1);

                if (_playerMovement.JumpReleasedDuringBufferTimer)
                {
                    _playerMovement.IsFastFalling = true;
                    _playerMovement.FastFallReleaseSpeed = _playerMovement.VerticalVelocity;
                }
            }

            //Double jump logic
            else if (_playerMovement.JumpBufferTimer > 0f && (_playerMovement.IsJumping || _playerMovement.IsWallJumping || _playerMovement.IsWallSlideFalling || _playerMovement.IsAirDashing || _playerMovement.IsDashFastFalling) && !_playerMovement.IsTouchingWall && _playerMovement.NumberOfJumpsUsed < _playerMovement.MovementStats.NumberOfJumpsAllowed)
            {
                _playerMovement.IsFastFalling = false;
                InitiateJump(1);

                if (_playerMovement.IsDashFastFalling)
                {
                    _playerMovement.IsDashFastFalling = false;
                }
            }

            //air jump after coyote time lapsed logic
            else if (_playerMovement.JumpBufferTimer > 0f && _playerMovement.IsFalling && !_playerMovement.IsWallSlideFalling && _playerMovement.NumberOfJumpsUsed < _playerMovement.MovementStats.NumberOfJumpsAllowed - 1)//fixed bugs of uneccesary double jump in air
            {
                InitiateJump(2); //because we are falling and it's air jump
                _playerMovement.IsFastFalling = false;
            }

        }

        private void InitiateJump(int numberOfJumpsUsed)
        {
            if (!_playerMovement.IsJumping)
            {
                _playerMovement.IsJumping = true;
            }

            _playerMovement.ResetWallJumpValues();

            _playerMovement.JumpBufferTimer = 0f;
            _playerMovement.NumberOfJumpsUsed += numberOfJumpsUsed;
            _playerMovement.VerticalVelocity = _playerMovement.MovementStats.InitialJumpVelocity;
        }
        public void Jump()
        {
            //apply gravity
            if (_playerMovement.IsJumping)
            {
                //check head bump
                if (_playerMovement.BumpedHead)
                {
                    _playerMovement.IsFastFalling = true;

                }

                //gravity of ascending
                if (_playerMovement.VerticalVelocity >= 0f)
                {
                    //apex controls
                    _playerMovement.ApexPoint = Mathf.InverseLerp(_playerMovement.MovementStats.InitialJumpVelocity, 0f, _playerMovement.VerticalVelocity);

                    if (_playerMovement.ApexPoint > _playerMovement.MovementStats.ApexThreshhold)
                    {
                        if (!_playerMovement.IsPastApexThreshold)
                        {
                            _playerMovement.IsPastApexThreshold = true;
                            _playerMovement.TimePastApexThreshold = 0f;
                        }
                        if (_playerMovement.IsPastApexThreshold)
                        {
                            _playerMovement.TimePastApexThreshold += Time.deltaTime;
                            if (_playerMovement.TimePastApexThreshold < _playerMovement.MovementStats.ApexHangTime)
                            {
                                _playerMovement.VerticalVelocity = 0f;
                            }
                            else
                            {
                                _playerMovement.VerticalVelocity = -0.01f;
                            }
                        }
                    }

                    //gravity of ascending not past apex threshold
                    else if (!_playerMovement.IsFastFalling)
                    {
                        _playerMovement.VerticalVelocity += _playerMovement.MovementStats.Gravity * Time.fixedDeltaTime;
                        if (_playerMovement.IsPastApexThreshold)
                        {
                            _playerMovement.IsPastApexThreshold = false;
                        }
                    }
                }

                //gravity of descending
                else if (!_playerMovement.IsFastFalling)
                {
                    _playerMovement.VerticalVelocity += _playerMovement.MovementStats.Gravity * _playerMovement.MovementStats.GravityReleaseMultiplier * Time.fixedDeltaTime;
                }
                else if (_playerMovement.VerticalVelocity < 0f)
                {
                    if (!_playerMovement.IsFalling)
                    {
                        _playerMovement.IsFalling = true;
                    }
                }
            }

            //jump cut
            if (_playerMovement.IsFastFalling)
            {
                if (_playerMovement.FastFallTime >= _playerMovement.MovementStats.TimeForUpwardsCancel)
                {
                    _playerMovement.VerticalVelocity += _playerMovement.MovementStats.Gravity * _playerMovement.MovementStats.GravityReleaseMultiplier * Time.fixedDeltaTime;
                }
                else if (_playerMovement.FastFallTime < _playerMovement.MovementStats.TimeForUpwardsCancel)
                {
                    _playerMovement.VerticalVelocity = Mathf.Lerp(_playerMovement.FastFallReleaseSpeed, 0f, (_playerMovement.FastFallTime / _playerMovement.MovementStats.TimeForUpwardsCancel));
                }
                _playerMovement.FastFallTime += Time.fixedDeltaTime;
            }
        }
    }
}

