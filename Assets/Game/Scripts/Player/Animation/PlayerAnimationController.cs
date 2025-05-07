using DG.Tweening;
using UnityEngine;
using UniRx;
using Zenject;
using System;
using PlayerMovement;
using PlayerAttack;
using PlayerEvent;

namespace PlayerAnimation
{
    /// <summary>
    ///  Чистый (не‑MonoBehaviour) контроллер анимаций игрока.
    ///  Обновляется через ITickable, получает события через IPlayerEventNotifier.
    /// </summary>
    public class PlayerAnimationController : ITickable, IDisposable
    {
        private readonly Animator _animator;
        private readonly PlayerMovementModel _model;
        private readonly CompositeDisposable _subs = new();

        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int WallSlide = Animator.StringToHash("WallSlide");
        private static readonly int Attack = Animator.StringToHash("Attack");

        public PlayerAnimationController(Animator animator,
                                         PlayerMovementModel model,
                                         IPlayerEventNotifier events)
        {
            _animator = animator;
            _model = model;

            // события
            events.OnAttack.Subscribe(_ => _animator.SetTrigger(Attack)).AddTo(_subs);
            events.OnJump.Subscribe(_ => { /* можно добавить jump‑fx */ }).AddTo(_subs);
        }

        public void Tick() => UpdateStates();

        private void UpdateStates()
        {
            bool running = Mathf.Abs(_model.HorizontalVelocity) > 0.1f && _model.IsGrounded;
            bool jumping = _model.IsJumping;
            bool falling = _model.IsFalling && !_model.IsGrounded;
            bool wallSliding = _model.IsWallSliding;

            _animator.SetBool(Run, running);
            _animator.SetBool(Jump, jumping);
            _animator.SetBool(Fall, falling);
            _animator.SetBool(WallSlide, wallSliding);
        }

        public void Dispose() => _subs.Dispose();
    }
}


