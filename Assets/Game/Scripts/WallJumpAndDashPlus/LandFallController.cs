using UnityEngine;

namespace PlayerMovementRefactoring
{
    public class LandFallController
    {
        private readonly PlayerMovement _playerMovement;

        public LandFallController(PlayerMovement player)
        {
            _playerMovement = player;
        }
        public void LandCheck()
        {
            //landed logic
            if ((_playerMovement.IsJumping || _playerMovement.IsFalling || _playerMovement.IsWallJumping || _playerMovement.IsWallJumpFalling || _playerMovement.IsWallSlideFalling || _playerMovement.IsWallSliding || _playerMovement.IsDashFastFalling) && _playerMovement.IsGrounded && _playerMovement.VerticalVelocity <= 0f)
            {
                _playerMovement.ResetJumpValues();
                _playerMovement.StopWallSlide();
                _playerMovement.ResetWallJumpValues();
                _playerMovement.ResetDashes();
                _playerMovement.NumberOfJumpsUsed = 0;
                _playerMovement.VerticalVelocity = Physics2D.gravity.y;

                if (_playerMovement.IsDashFastFalling && _playerMovement.IsGrounded)
                {
                    _playerMovement.ResetDashValues();
                    return;
                }

                _playerMovement.ResetDashValues();
            }
        }

        public void Fall()
        {
            //normal gravity while falling
            if (!_playerMovement.IsGrounded && !_playerMovement.IsJumping && !_playerMovement.IsWallSliding && !_playerMovement.IsWallJumping && !_playerMovement.IsDashing && !_playerMovement.IsDashFastFalling)
            {
                if (!_playerMovement.IsFalling)
                {
                    _playerMovement.IsFalling = true;
                }
                _playerMovement.VerticalVelocity += _playerMovement.MovementStats.Gravity * Time.fixedDeltaTime;
            }
        }
    }


}