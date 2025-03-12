using UnityEngine;

namespace PlayerMovementRefactoring
{
    public class WallSlideController
    {
        private readonly PlayerMovement _playerMovement;

        public WallSlideController(PlayerMovement player)
        {
            _playerMovement = player;;
        }
        public void WallSlideCheck()
        {
            if (_playerMovement.IsTouchingWall && !_playerMovement.IsGrounded && !_playerMovement.IsDashing)
            {
                if (_playerMovement.VerticalVelocity < 0f && !_playerMovement.IsWallSliding)
                {
                    _playerMovement.ResetJumpValues();
                    _playerMovement.ResetWallJumpValues();
                    _playerMovement.ResetDashValues();

                    if (_playerMovement.MovementStats.ResetDashOnWallSlide)
                    {
                        _playerMovement.ResetDashes();
                    }

                    _playerMovement.IsWallSlideFalling = false;
                    _playerMovement.IsWallSliding = true;

                    if (_playerMovement.MovementStats.ResetJumpOnWallSlide)
                    {
                        _playerMovement.NumberOfJumpsUsed = 0;
                    }
                }
            }

            else if (_playerMovement.IsWallSliding && !_playerMovement.IsTouchingWall && !_playerMovement.IsGrounded && !_playerMovement.IsWallSlideFalling)
            {
                _playerMovement.IsWallSlideFalling = true;
                _playerMovement.StopWallSlide();
            }
            else
            {
                _playerMovement.StopWallSlide();
            }
        }


        public void WallSlide()
        {
            if (_playerMovement.IsWallSliding)
            {
                _playerMovement.VerticalVelocity = Mathf.Lerp(_playerMovement.VerticalVelocity, -_playerMovement.MovementStats.WallSlideSpeed, _playerMovement.MovementStats.WallSlideDecelerationSpeed * Time.fixedDeltaTime);

            }
        }
    }
}