// === PlayerMovementModel.cs ===
using UnityEngine;

namespace PlayerMovementLogic
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
        [HideInInspector] public float HorizontalVelocity;
        public bool IsFacingRight = true;

        //collision check variables
        [HideInInspector] public RaycastHit2D GroundHit;
        [HideInInspector] public RaycastHit2D HeadHit;
        public bool IsGrounded;
        [HideInInspector] public bool BumpedHead;

        //wall collision check variables
        [HideInInspector] public RaycastHit2D WallHit;
        [HideInInspector] public RaycastHit2D LastWallHit;
        public bool IsTouchingWall;

        //jump variables
        [HideInInspector] public float VerticalVelocity;
        [HideInInspector] public bool IsJumping;
        [HideInInspector] public bool IsFastFalling;
        [HideInInspector] public bool IsFalling;
        [HideInInspector] public float FastFallTime;
        [HideInInspector] public float FastFallReleaseSpeed;
        [HideInInspector] public int NumberOfJumpsUsed;

        //apex variables
        [HideInInspector] public float ApexPoint;
        [HideInInspector] public float TimePastApexThreshold;
        [HideInInspector] public bool IsPastApexThreshold;

        //jump buffer vars
        [HideInInspector] public float JumpBufferTimer;
        [HideInInspector] public bool JumpReleasedDuringBufferTimer;

        //coyote time vars
        [HideInInspector] public float CoyoteTimer;

        //wall slide
        public bool IsWallSliding;
        [HideInInspector] public bool IsWallSlideFalling;

        //wall jump
        [HideInInspector] public bool UseWallJumpMoveStats;
        public bool IsWallJumping;
        [HideInInspector] public float WallJumpTime;
        [HideInInspector] public bool IsWallJumpFastFalling;
        [HideInInspector] public bool IsWallJumpFalling;
        [HideInInspector] public float WallJUmpFastFallTime;
        [HideInInspector] public float WallJumpFastFallReleaseSpeed;

        [HideInInspector] public float WallJumpPostBufferTimer;

        [HideInInspector] public float WallJumpApexPoint;
        [HideInInspector] public float TimePastWallJumpApexThreshold;
        [HideInInspector] public bool IsPastWallJumpApexThreshold;

        //dash vars
        public bool IsDashing;
        public bool IsAirDashing;
        [HideInInspector] public float DashTimer;
        [HideInInspector] public float DashOnGroundTimer;
        [HideInInspector] public int NumberOfDashesUsed;
        [HideInInspector] public Vector2 DashDirection;
        [HideInInspector] public bool IsDashFastFalling;
        [HideInInspector] public float DashFastFallTime;
        [HideInInspector] public float DashFastFallReleaseSpeed;


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
