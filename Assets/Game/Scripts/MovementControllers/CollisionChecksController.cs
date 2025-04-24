using UnityEngine;

namespace PlayerMovementLogic
{
    public class CollisionChecksController
    {
        private readonly PlayerMovementModel _model;
        private readonly PlayerMovementController _controller;
        private readonly Transform _transform;

        public CollisionChecksController(PlayerMovementModel model, PlayerMovementController controller, Transform transform)
        {
            _model = model;
            _controller = controller;
            _transform = transform;
        }

        public void CollisionChecks()
        {
            CheckIsGrounded();
            CheckBumpedHead();
            CheckIsTouchingWall();
        }

        private void CheckIsGrounded()
        {
            Vector2 boxCastOrigin = new(_model.FeetCollider.bounds.center.x, _model.FeetCollider.bounds.min.y);
            Vector2 boxCastSize = new(_model.FeetCollider.bounds.size.x, _model.MovementStats.GroundDetectionRayLength);

            _model.GroundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, _model.MovementStats.GroundDetectionRayLength, _model.MovementStats.GroundLayer);
            _model.IsGrounded = _model.GroundHit.collider != null;
        }

        private void CheckBumpedHead()
        {
            Vector2 boxCastOrigin = new(_model.FeetCollider.bounds.center.x, _model.BodyCollider.bounds.max.y);
            Vector2 boxCastSize = new(_model.FeetCollider.bounds.size.x * _model.MovementStats.HeadWidth, _model.MovementStats.HeadDetectionRayLength);

            _model.HeadHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, _model.MovementStats.HeadDetectionRayLength, _model.MovementStats.GroundLayer);
            _model.BumpedHead = _model.HeadHit.collider != null;
        }

        private void CheckIsTouchingWall()
        {
            float originEndPoint = _model.IsFacingRight
                ? _model.BodyCollider.bounds.max.x
                : _model.BodyCollider.bounds.min.x;

            float adjustedHeight = _model.BodyCollider.bounds.size.y * _model.MovementStats.WallDetectionRayHeightMultiplier;

            Vector2 boxCastOrigin = new(originEndPoint, _model.BodyCollider.bounds.center.y);
            Vector2 boxCastSize = new(_model.MovementStats.WallDetectionRayLength, adjustedHeight);

            _model.WallHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, _model.IsFacingRight ? Vector2.right : Vector2.left, _model.MovementStats.WallDetectionRayLength, _model.MovementStats.GroundLayer);
            if (_model.WallHit.collider != null)
            {
                _model.LastWallHit = _model.WallHit;
                _model.IsTouchingWall = true;
            }
            else
            {
                _model.IsTouchingWall = false;
            }
        }
    }
}