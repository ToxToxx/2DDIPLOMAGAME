// === PlayerMovementController.cs ===
using UnityEngine;

namespace PlayerMovementLogic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Model")]
        public PlayerMovementModel Model;

        private Rigidbody2D _playerRigidbody;
        private GroundMovement _groundMovement;
        private JumpHandler _jumpHandler;
        private LandFallController _landFallController;
        private WallSlideController _wallSlideController;
        private WallJumpController _wallJumpController;
        private DashController _dashController;
        private CollisionChecksController _collisionChecksController;
        private TimerController _timerController;

        private void Awake()
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
            _groundMovement = new GroundMovement(Model, this);
            _jumpHandler = new JumpHandler(Model, this);
            _landFallController = new LandFallController(Model, this);
            _wallSlideController = new WallSlideController(Model, this);
            _wallJumpController = new WallJumpController(Model, this);
            _dashController = new DashController(Model, this);
            _collisionChecksController = new CollisionChecksController(Model, this);
            _timerController = new TimerController(Model, _wallJumpController);
        }

        private void Update()
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

        private void FixedUpdate()
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
            transform.rotation = Quaternion.Euler(0, right ? 0 : 180, 0);
        }


    }
}
