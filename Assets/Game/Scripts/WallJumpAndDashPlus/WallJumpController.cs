using UnityEngine;


namespace PlayerMovementRefactoring
{
    public class WallJumpController
    {
        private readonly PlayerMovement _playerMovement;

        public WallJumpController(PlayerMovement player)
        {
            _playerMovement = player;
        }
        public void WallJumpCheck()
        {
            if (ShouldApplyPostWallJumpBuffer())
            {
                _playerMovement.WallJumpPostBufferTimer = _playerMovement.MovementStats.WallJumpPostBufferTime;
            }

            //wall jump fast falling
            if (InputManager.JumpWasReleased && !_playerMovement.IsWallSliding && !_playerMovement.IsTouchingWall && _playerMovement.IsWallJumping)
            {
                if (_playerMovement.VerticalVelocity > 0f)
                {
                    if (_playerMovement.IsPastWallJumpApexThreshold)
                    {
                        _playerMovement.IsPastWallJumpApexThreshold = false;
                        _playerMovement.IsWallJumpFastFalling = true;
                        _playerMovement.WallJUmpFastFallTime = _playerMovement.MovementStats.TimeForUpwardsCancel;

                        _playerMovement.VerticalVelocity = 0F;
                    }
                    else
                    {
                        _playerMovement.IsWallJumpFastFalling = true;
                        _playerMovement.WallJumpFastFallReleaseSpeed = _playerMovement.VerticalVelocity;
                    }
                }
            }

            //actual jump with post wall jump buffer time
            if (InputManager.JumpWasPressed && _playerMovement.WallJumpPostBufferTimer > 0f)
            {
                InitialWallJump();
            }
        }

        private void InitialWallJump()
        {
            if (!_playerMovement.IsWallJumping)
            {
                _playerMovement.IsWallJumping = true;
                _playerMovement.UseWallJumpMoveStats = true;
            }

            _playerMovement.StopWallSlide();
            _playerMovement.ResetJumpValues();
            _playerMovement.WallJumpTime = 0f;

            _playerMovement.VerticalVelocity = _playerMovement.MovementStats.InitialWallJumpVelocity;

            int dirMultiplier = 0;
            Vector2 hitPoint = _playerMovement.LastWallHit.collider.ClosestPoint(_playerMovement.BodyCollider.bounds.center);

            if (hitPoint.x > _playerMovement.transform.position.x)
            {
                dirMultiplier = -1;
            }
            else dirMultiplier = 1;

            _playerMovement.HorizontalVelocity = Mathf.Abs(_playerMovement.MovementStats.WallJumpDirection.x) * dirMultiplier;
        }

        public void WallJump()
        {
            //Apply Wall Jump Gravity
            if (_playerMovement.IsWallJumping)
            {
                _playerMovement.WallJumpTime += Time.fixedDeltaTime;
                if (_playerMovement.WallJumpTime >= _playerMovement.MovementStats.TimeTillJumpApex)
                {
                    _playerMovement.UseWallJumpMoveStats = false;
                }

                //Hit head
                if (_playerMovement.BumpedHead)
                {
                    _playerMovement.IsWallJumpFastFalling = true;
                    _playerMovement.UseWallJumpMoveStats = false;
                }

                //Gravity In Ascending
                if (_playerMovement.VerticalVelocity >= 0f)
                {
                    //Apex controls
                    _playerMovement.WallJumpApexPoint = Mathf.InverseLerp(_playerMovement.MovementStats.WallJumpDirection.y, 0f, _playerMovement.VerticalVelocity);

                    if (_playerMovement.WallJumpApexPoint > _playerMovement.MovementStats.ApexThreshhold)
                    {
                        if (!_playerMovement.IsPastWallJumpApexThreshold)
                        {
                            _playerMovement.IsPastWallJumpApexThreshold = true;
                            _playerMovement.TimePastWallJumpApexThreshold = 0f;
                        }
                        if (_playerMovement.IsPastWallJumpApexThreshold)
                        {
                            _playerMovement.TimePastWallJumpApexThreshold += Time.fixedDeltaTime;
                            if (_playerMovement.TimePastWallJumpApexThreshold < _playerMovement.MovementStats.ApexHangTime)
                            {
                                _playerMovement.VerticalVelocity = 0f;
                            }
                            else
                            {
                                _playerMovement.VerticalVelocity = -0.01f;
                            }
                        }
                    }
                    // Gravity in ascending but not past apex threshold
                    else if (!_playerMovement.IsWallJumpFastFalling)
                    {
                        _playerMovement.VerticalVelocity += _playerMovement.MovementStats.WallJumpGravity * Time.fixedDeltaTime;

                        if (_playerMovement.IsPastWallJumpApexThreshold)
                        {
                            _playerMovement.IsPastWallJumpApexThreshold = false;
                        }
                    }
                }
                // Gravity in Descending
                else if (!_playerMovement.IsWallJumpFastFalling)
                {
                    _playerMovement.VerticalVelocity += _playerMovement.MovementStats.WallJumpGravity * Time.fixedDeltaTime;
                }

                else if (_playerMovement.VerticalVelocity < 0f)
                {
                    if (!_playerMovement.IsWallJumpFalling)
                        _playerMovement.IsWallJumpFalling = true;
                }
            }

            //handle wall jump cut time
            if (_playerMovement.IsWallJumpFastFalling)
            {
                if (_playerMovement.WallJUmpFastFallTime >= _playerMovement.MovementStats.TimeForUpwardsCancel)
                {
                    _playerMovement.VerticalVelocity += _playerMovement.MovementStats.WallJumpGravity * _playerMovement.MovementStats.WallJumpGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
                else if (_playerMovement.WallJUmpFastFallTime < _playerMovement.MovementStats.TimeForUpwardsCancel)
                {
                    _playerMovement.VerticalVelocity = Mathf.Lerp(_playerMovement.WallJumpFastFallReleaseSpeed, 0f, (_playerMovement.WallJUmpFastFallTime / _playerMovement.MovementStats.TimeForUpwardsCancel));
                }

                _playerMovement.WallJUmpFastFallTime += Time.fixedDeltaTime;
            }
        }

        public bool ShouldApplyPostWallJumpBuffer()
        {
            if (!_playerMovement.IsGrounded && (_playerMovement.IsTouchingWall || _playerMovement.IsWallSliding))
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

