using InGameInput;
using UnityEngine;

namespace PlayerMovement
{
    public class GroundMovement
    {
        private readonly PlayerMovementModel _model;
        private readonly PlayerMovementController _controller;
        private readonly IInputService _input;

        public GroundMovement(PlayerMovementModel model, PlayerMovementController controller, IInputService input)
        {
            _model = model;
            _controller = controller;
            _input = input;
        }

        public void Move(float acceleration, float deceleration, Vector2 moveInput)
        {
            if (!_model.IsDashing)
            {
                if (Mathf.Abs(moveInput.x) >= _model.MovementStats.MoveTreshold)
                {
                    _controller.TurnCheck(moveInput); // оставляем во внешнем компоненте поворот

                    float targetVelocity = _input.RunIsHeld
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
