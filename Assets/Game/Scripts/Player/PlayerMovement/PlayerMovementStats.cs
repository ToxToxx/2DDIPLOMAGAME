using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")]
public class PlayerMovementStats : ScriptableObject
{
    [Header("Walk")]
    [Range(0f, 1f)] public float MoveTreshold = 0.25f;
    [Range(1f, 100f)] public float MaxWalkSpeed = 12.5f;
    [Range(0.25f, 50f)] public float GroundAcceleration = 5f;
    [Range(0.25f, 50f)] public float GroundDeceleration = 20f;
    [Range(0.25f, 50f)] public float AirAcceleration = 5f;
    [Range(0.25f, 50f)] public float AirDeceleration = 5f;

    [Header("Wall Acceleration")]
    [Range(0.25f, 50f)] public float WallJumpMoveAceleration = 5f;
    [Range(0.25f, 50f)] public float WallJumpMoveDeceleration= 5f;

    [Header("Run")]
    [Range(1f, 100f)] public float MaxRunSpeed = 20f;

    [Header("Grounded/Collision Checks")]
    public LayerMask GroundLayer;
    public float GroundDetectionRayLength = 0.02f;
    public float HeadDetectionRayLength = 0.02f;
    [Range(0f, 1f)] public float HeadWidth = 0.75f;

    [Header("Wall collision checks")]
    public float WallDetectionRayLength = 0.125f;
    [Range(0.01f, 2f)] public float WallDetectionRayHeightMultiplier = 0.9f;

    [Header("Jump")]
    public float JumpHeight = 6.5f;
    [Range(1f, 1.1f)] public float JumpHeightCompensationFactor = 1.054f;
    public float TimeTillJumpApex = 0.35f;
    [Range(0.01f, 5f)] public float GravityReleaseMultiplier = 2f;
    public float MaxFallSpeed = 26f;
    [Range(1, 5)] public int NumberOfJumpsAllowed = 2;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float TimeForUpwardsCancel = 0.027f;

    [Header("Jump Apex")]
    [Range(0.5f, 1f)] public float ApexThreshhold = 0.97f;
    [Range(0.01f, 1f)] public float ApexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)] public float JumpBufferTime = 0.125f;

    [Header("Jump Coyote Time")]
    [Range(0f, 1f)] public float JumpCoyoteTime = 0.1f;

    [Header("Reset Jump Option")]
    public bool ResetJumpOnWallSlide = true;

    [Header("Wall slide")]
    [Min(0.01f)] public float WallSlideSpeed = 5f;
    [Range(0.25f, 50f)] public float WallSlideDecelerationSpeed = 50f;

    [Header("Wall jump")]
    public Vector2 WallJumpDirection = new Vector2(-20f, 6.5f);
    [Range(0f, 1f)] public float WallJumpPostBufferTime = 0.125f;
    [Range(0.01f, 5f)] public float WallJumpGravityOnReleaseMultiplier = 1f;

    [Header("Dash")]
    [Range(0f, 1f)] public float DashTime = 0.11f;
    [Range(1f, 200f)] public float DashSpeed = 40f;
    [Range(0f, 1f)] public float TimeBtwDashesOnGround = 0.225f;
    public bool ResetDashOnWallSlide = true;
    [Range(0, 5)] public int NumberOfDashes = 2;
    [Range(0f, 0.5f)] public float DashDiagonallyBias = 0.4f;

    [Header("Dash Cancel Time")]
    [Range(0.01f, 5f)] public float DashGravityOnReleaseMultiplier = 1f;
    [Range(0.02f, 0.3f)] public float DashTimeForUpwardsCancel = 0.027f;

    public readonly Vector2[] DashDirections = new Vector2[]
    {
        new Vector2 (0, 0), //Nothing
        new Vector2 (1, 0), //Right
        new Vector2 (1, 1).normalized, //Top-Right
        new Vector2 (0, 1), //Up
        new Vector2 (-1, 1).normalized, //Top-Left
        new Vector2 (-1, 0), //Left
        new Vector2 (-1, -1).normalized, //Bottom-Left
        new Vector2 (0, -1), //Down
        new Vector2 (1, -1).normalized, //Bottom-Right
    };

    //Jump
    public float Gravity { get; private set; }
    public float InitialJumpVelocity { get; private set; }
    public float AdjustedJumpHeight { get; private set; }

    //Wall Jump
     public float WallJumpGravity { get; private set; }
    public float InitialWallJumpVelocity { get; private set; }
    public float AdjustedWallJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateJumpValues();
        CalculateWallJumpValues();
    }

    private void OnEnable()
    {
        CalculateJumpValues();
        CalculateWallJumpValues();
    }

    private void CalculateJumpValues()
    {
        AdjustedJumpHeight = JumpHeight * JumpHeightCompensationFactor;
        Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(TimeTillJumpApex, 2f);
        InitialJumpVelocity = Mathf.Abs(Gravity) * TimeTillJumpApex;     
    }

    private void CalculateWallJumpValues()
    {
        AdjustedWallJumpHeight = WallJumpDirection.y * JumpHeightCompensationFactor;
        WallJumpGravity = -(2f * AdjustedWallJumpHeight) / Mathf.Pow(TimeTillJumpApex, 2f);
        InitialWallJumpVelocity = Mathf.Abs(WallJumpGravity) * TimeTillJumpApex;
    }

}
