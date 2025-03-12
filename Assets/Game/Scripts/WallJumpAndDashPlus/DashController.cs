using UnityEngine;

namespace PlayerMovementRefactoring
{
    public class DashController
    {
        private readonly PlayerMovement _playerMovement;

        public DashController(PlayerMovement player)
        {
            _playerMovement = player;
        }
        public void DashCheck()
        {
            if (InputManager.DashWasPressed)
            {
                //ground dash
                if (_playerMovement.IsGrounded && _playerMovement.DashOnGroundTimer < 0 && !_playerMovement.IsDashing)
                {
                    InitiateDash();
                }

                //air dash
                else if (!_playerMovement.IsGrounded && !_playerMovement.IsDashing && _playerMovement.NumberOfDashesUsed < _playerMovement.MovementStats.NumberOfDashes)
                {
                    _playerMovement.IsAirDashing = true;
                    InitiateDash();

                    //you left a wallslide but dashed within the wall jump post buffer timer
                    if (_playerMovement.WallJumpPostBufferTimer > 0f)
                    {
                        _playerMovement.NumberOfJumpsUsed--;
                        if (_playerMovement.NumberOfJumpsUsed < 0f)
                        {
                            _playerMovement.NumberOfJumpsUsed = 0;
                        }
                    }
                }
            }
        }

        private void InitiateDash()
        {
            _playerMovement.DashDirection = InputManager.Movement;

            Vector2 closestDirection = Vector2.zero;
            float minDistance = Vector2.Distance(_playerMovement.DashDirection, _playerMovement.MovementStats.DashDirections[0]);

            for (int i = 0; i < _playerMovement.MovementStats.DashDirections.Length; i++)
            {
                //skip if we hit it bang on
                if (_playerMovement.DashDirection == _playerMovement.MovementStats.DashDirections[i])
                {
                    closestDirection = _playerMovement.DashDirection;
                    break;
                }

                float distance = Vector2.Distance(_playerMovement.DashDirection, _playerMovement.MovementStats.DashDirections[i]);

                //if diagonal direction
                bool isDiagonal = (Mathf.Abs(_playerMovement.MovementStats.DashDirections[i].x) == 1 && Mathf.Abs(_playerMovement.MovementStats.DashDirections[i].y) == 1);
                if (isDiagonal)
                {
                    distance -= _playerMovement.MovementStats.DashDiagonallyBias;
                }
                else if (distance < minDistance)
                {
                    minDistance = distance;
                    closestDirection = _playerMovement.MovementStats.DashDirections[i];
                }
            }

            //handle direction with no input
            if (closestDirection == Vector2.zero)
            {
                if (_playerMovement.IsFacingRight)
                {
                    closestDirection = Vector2.right;
                }
                else closestDirection = Vector2.left;
            }

            _playerMovement.DashDirection = closestDirection;
            _playerMovement.NumberOfDashesUsed++;
            _playerMovement.IsDashing = true;
            _playerMovement.DashTimer = 0f;
            _playerMovement.DashOnGroundTimer = _playerMovement.MovementStats.TimeBtwDashesOnGround;

            _playerMovement.ResetJumpValues();
            _playerMovement.ResetWallJumpValues();
            _playerMovement.StopWallSlide();
        }
        public void Dash()
        {
            if (_playerMovement.IsDashing)
            {
                //stop the dash after the timer
                _playerMovement.DashTimer += Time.fixedDeltaTime;
                if (_playerMovement.DashTimer >= _playerMovement.MovementStats.DashTime)
                {
                    if (_playerMovement.IsGrounded)
                    {
                        _playerMovement.ResetDashes();
                    }

                    _playerMovement.IsAirDashing = false;
                    _playerMovement.IsDashing = false;

                    if (!_playerMovement.IsJumping && !_playerMovement.IsWallJumping)
                    {
                        _playerMovement.DashFastFallTime = 0f;
                        _playerMovement.DashFastFallReleaseSpeed = _playerMovement.VerticalVelocity;

                        if (!_playerMovement.IsGrounded)
                        {
                            _playerMovement.IsDashFastFalling = true;
                        }
                    }

                    return;
                }

                _playerMovement.HorizontalVelocity = _playerMovement.MovementStats.DashSpeed * _playerMovement.DashDirection.x;

                if (_playerMovement.DashDirection.y != 0f || _playerMovement.IsAirDashing)
                {
                    _playerMovement.VerticalVelocity = _playerMovement.MovementStats.DashSpeed * _playerMovement.DashDirection.y;
                }
            }

            //Handle dash cut time
            else if (_playerMovement.IsDashFastFalling)
            {
                if (_playerMovement.VerticalVelocity > 0f)
                {
                    if (_playerMovement.DashFastFallTime < _playerMovement.MovementStats.DashTimeForUpwardsCancel)
                    {
                        _playerMovement.VerticalVelocity = Mathf.Lerp(_playerMovement.DashFastFallReleaseSpeed, 0f, (_playerMovement.DashFastFallTime / _playerMovement.MovementStats.DashTimeForUpwardsCancel));
                    }
                    else if (_playerMovement.DashFastFallTime >= _playerMovement.MovementStats.DashTimeForUpwardsCancel)
                    {
                        _playerMovement.VerticalVelocity += _playerMovement.MovementStats.Gravity * _playerMovement.MovementStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                    }

                    _playerMovement.DashFastFallTime += Time.fixedDeltaTime;
                }
                else
                {
                    _playerMovement.VerticalVelocity += _playerMovement.MovementStats.Gravity * _playerMovement.MovementStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
            }
        }


    }

}