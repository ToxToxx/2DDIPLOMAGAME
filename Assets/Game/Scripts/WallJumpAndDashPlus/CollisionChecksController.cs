using UnityEngine;


namespace PlayerMovementRefactoring
{
    public class CollisionChecksController
    {
        private readonly PlayerMovement _playerMovement;

        public CollisionChecksController(PlayerMovement player)
        {
            _playerMovement = player;
        }

        public void CollisionChecks()
        {
            IsGrounded();
            BumpedHead();
            IsTouchingWall();
        }

        private void IsGrounded()
        {
            Vector2 boxCastOrigin = new(_playerMovement.FeetCollider.bounds.center.x, _playerMovement.FeetCollider.bounds.min.y);
            Vector2 boxCastSize = new(_playerMovement.FeetCollider.bounds.size.x, _playerMovement.MovementStats.GroundDetectionRayLength);

            _playerMovement.GroundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, _playerMovement.MovementStats.GroundDetectionRayLength, _playerMovement.MovementStats.GroundLayer);
            _playerMovement.IsGrounded = _playerMovement.GroundHit.collider != null;
        }

        private void BumpedHead()
        {
            Vector2 boxCastOrigin = new(_playerMovement.FeetCollider.bounds.center.x, _playerMovement.BodyCollider.bounds.max.y);
            Vector2 boxCastSize = new(_playerMovement.FeetCollider.bounds.size.x * _playerMovement.MovementStats.HeadWidth, _playerMovement.MovementStats.HeadDetectionRayLength);

            _playerMovement.HeadHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, _playerMovement.MovementStats.HeadDetectionRayLength, _playerMovement.MovementStats.GroundLayer);
            _playerMovement.BumpedHead = _playerMovement.HeadHit.collider != null;
        }

        private void IsTouchingWall()
        {
            float originEndPoint = _playerMovement.IsFacingRight
                ? _playerMovement.BodyCollider.bounds.max.x
                : _playerMovement.BodyCollider.bounds.min.x;

            float adjustedHeight = _playerMovement.BodyCollider.bounds.size.y * _playerMovement.MovementStats.WallDetectionRayHeightMultiplier;

            Vector2 boxCastOrigin = new(originEndPoint, _playerMovement.BodyCollider.bounds.center.y);
            Vector2 boxCastSize = new(_playerMovement.MovementStats.WallDetectionRayLength, adjustedHeight);

            _playerMovement.WallHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, _playerMovement.transform.right, _playerMovement.MovementStats.WallDetectionRayLength, _playerMovement.MovementStats.GroundLayer);
            if (_playerMovement.WallHit.collider != null)
            {
                _playerMovement.LastWallHit = _playerMovement.WallHit;
                _playerMovement.IsTouchingWall = true;
            }
            else
            {
                _playerMovement.IsTouchingWall = false;
            }
        }
    }
}
