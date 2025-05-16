using UnityEngine;

namespace PlayerMovement
{
    [System.Serializable]
    public class PlayerMovementModel
    {
        [Header("References")]
        public PlayerMovementStats MovementStats;
        public Collider2D FeetCollider;
        public Collider2D BodyCollider;

        private Rigidbody2D _playerRigidbody;

        private GroundMovement _groundMovement;
        private JumpHandler _jumpHandler;
        private LandFallController _landFallController;
        private WallSlideController _wallSlideController;
        private WallJumpController _wallJumpController;
        private DashController _dashController;
        private CollisionChecksController _collisionChecksController;
        private TimerController _timerController;

        [Header("Variables")]
        // movement variables
        public float HorizontalVelocity { get; set; }
        public bool IsFacingRight { get; set; } = true;

        //collision check variables
        public RaycastHit2D GroundHit { get; set; }
        public RaycastHit2D HeadHit { get; set; }
        public bool IsGrounded { get; set; }
        public bool BumpedHead { get; set; }

        //wall collision check variables
        public RaycastHit2D WallHit { get; set; }
        public RaycastHit2D LastWallHit { get; set; }
        public bool IsTouchingWall { get; set; }

        //jump variables
        public float VerticalVelocity { get; set; }
        public bool IsJumping { get; set; }
        public bool IsFastFalling { get; set; }
        public bool IsFalling { get; set; }
        public float FastFallTime { get; set; }
        public float FastFallReleaseSpeed { get; set; }
        public int NumberOfJumpsUsed { get; set; }

        //apex variables
        public float ApexPoint { get; set; }
        public float TimePastApexThreshold { get; set; }
        public bool IsPastApexThreshold { get; set; }

        //jump buffer vars
        public float JumpBufferTimer { get; set; }
        public bool JumpReleasedDuringBufferTimer { get; set; }

        //coyote time vars
        public float CoyoteTimer { get; set; }

        //wall slide
        public bool IsWallSliding { get; set; }
        public bool IsWallSlideFalling { get; set; }

        //wall jump
        public bool UseWallJumpMoveStats { get; set; }
        public bool IsWallJumping { get; set; }
        public float WallJumpTime { get; set; }
        public bool IsWallJumpFastFalling { get; set; }
        public bool IsWallJumpFalling { get; set; }
        public float WallJumpFastFallTime { get; set; }
        public float WallJumpFastFallReleaseSpeed { get; set; }

        public float WallJumpPostBufferTimer { get; set; }

        public float WallJumpApexPoint { get; set; }
        public float TimePastWallJumpApexThreshold { get; set; }
        public bool IsPastWallJumpApexThreshold { get; set; }


        //dash vars
        public bool IsDashing { get; set; }
        public bool IsAirDashing { get; set; }
        public float DashTimer { get; set; }
        public float DashOnGroundTimer { get; set; }
        public int NumberOfDashesUsed { get; set; }
        public Vector2 DashDirection { get; set; }
        public bool IsDashFastFalling { get; set; }
        public float DashFastFallTime { get; set; }
        public float DashFastFallReleaseSpeed { get; set; }


        // Ability unlocks
        [Header("Ability Unlocks")]
        public bool CanWallSlide;
        public bool CanWallJump;
        public bool CanDash;

        public void ResetDashValues()
        {
            IsDashFastFalling = false;
            DashOnGroundTimer = -0.01f;
        }

        public void ResetDashes()
        {
            NumberOfDashesUsed = 0;
        }

        public void ResetWallJumpValues()
        {
            IsWallSlideFalling = false;
            UseWallJumpMoveStats = false;
            IsWallJumping = false;
            IsWallJumpFastFalling = false;
            IsWallJumpFalling = false;
            IsPastWallJumpApexThreshold = false;

            WallJumpFastFallReleaseSpeed = 0f;
            WallJumpTime = 0f;
        }

        public void ResetJumpValues()
        {
            IsJumping = false;
            IsFalling = false;
            IsFastFalling = false;
            FastFallTime = 0f;
            IsPastApexThreshold = false;
        }
        public void StopWallSlide()
        {
            if (IsWallSliding)
            {
                NumberOfJumpsUsed++;

                IsWallSliding = false;
            }
        }
    }
}
