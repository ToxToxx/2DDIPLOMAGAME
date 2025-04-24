using UnityEngine;

namespace PlayerMovementLogic
{
    public class GroundMovement
    {
        private readonly PlayerMovementModel _model;
        private readonly PlayerMovementController _controller;

        public GroundMovement(PlayerMovementModel model, PlayerMovementController controller)
        {
            _model = model;
            _controller = controller;
        }

        public void Move(float acceleration, float deceleration, Vector2 moveInput)
        {
            if (!_model.IsDashing)
            {
                if (Mathf.Abs(moveInput.x) >= _model.MovementStats.MoveTreshold)
                {
                    _controller.TurnCheck(moveInput); // оставляем во внешнем компоненте поворот

                    float targetVelocity = InputManager.RunIsHeld
                        ? moveInput.x * _model.MovementStats.MaxRunSpeed
                        : moveInput.x * _model.MovementStats.MaxWalkSpeed;

                    _model.HorizontalVelocity = Mathf.Lerp(_model.HorizontalVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
                }
                else if (Mathf.Abs(moveInput.x) <= _model.MovementStats.MoveTreshold)
                {
                    _model.HorizontalVelocity = Mathf.Lerp(_model.HorizontalVelocity, 0f, deceleration * Time.fixedDeltaTime);
                }
            }
        }
    }
}
