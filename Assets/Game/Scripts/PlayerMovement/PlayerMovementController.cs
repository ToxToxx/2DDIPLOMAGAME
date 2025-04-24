using UnityEngine;
using Zenject;

namespace PlayerMovementLogic
{
    public class PlayerMovementController : ITickable, IFixedTickable
    {
        private readonly PlayerMovementModel Model;
        private readonly Rigidbody2D _playerRigidbody;
        private readonly Transform _transform;
        private readonly GroundMovement _groundMovement;
        private readonly JumpHandler _jumpHandler;
        private readonly LandFallController _landFallController;
        private readonly WallSlideController _wallSlideController;
        private readonly WallJumpController _wallJumpController;
        private readonly DashController _dashController;
        private readonly CollisionChecksController _collisionChecksController;
        private readonly TimerController _timerController;

        public PlayerMovementController(PlayerMovementModel model, Rigidbody2D playerRigidbody, Transform transform)
        {
            Model = model;
            _playerRigidbody = playerRigidbody;
            _transform = transform;

            _groundMovement = new GroundMovement(Model, this);
            _jumpHandler = new JumpHandler(Model, this);
            _landFallController = new LandFallController(Model, this);
            _wallSlideController = new WallSlideController(Model, this);
            _wallJumpController = new WallJumpController(Model, this, _transform);
            _dashController = new DashController(Model, this);
            _collisionChecksController = new CollisionChecksController(Model, this, _transform);
            _timerController = new TimerController(Model, _wallJumpController);
        }

        public Transform Transform => _transform;

        public void Tick()
        {
            _timerController.CountTimers();
            _jumpHandler.JumpChecks();
            _landFallController.LandCheck();

            if (Model.CanWallSlide)
                _wallSlideController.WallSlideCheck();
            if (Model.CanWallJump)
                _wallJumpController.WallJumpCheck();
            if (Model.CanDash)
                _dashController.DashCheck();
        }

        public void FixedTick()
        {
            _collisionChecksController.CollisionChecks();
            _jumpHandler.Jump();
            _landFallController.Fall();
            _wallSlideController.WallSlide();
            _wallJumpController.WallJump();
            _dashController.Dash();

            ApplyMovement();
            ApplyVelocity();
        }

        private void ApplyMovement()
        {
            float accel = Model.IsGrounded ?
                Model.MovementStats.GroundAcceleration :
                Model.MovementStats.AirAcceleration;
            float decel = Model.IsGrounded ?
                Model.MovementStats.GroundDeceleration :
                Model.MovementStats.AirDeceleration;

            _groundMovement.Move(accel, decel, InputManager.Movement);
        }

        private void ApplyVelocity()
        {
            float maxFall = Model.IsDashing ? -50f : -Model.MovementStats.MaxFallSpeed;
            Model.VerticalVelocity = Mathf.Clamp(Model.VerticalVelocity, maxFall, 50f);
            _playerRigidbody.linearVelocity = new Vector2(Model.HorizontalVelocity, Model.VerticalVelocity);
        }

        public void TurnCheck(Vector2 moveInput)
        {
            if (Model.IsFacingRight && moveInput.x < 0) Turn(false);
            else if (!Model.IsFacingRight && moveInput.x > 0) Turn(true);
        }

        private void Turn(bool right)
        {
            Model.IsFacingRight = right;
            _transform.rotation = Quaternion.Euler(0, right ? 0 : 180, 0);
        }
    }
}