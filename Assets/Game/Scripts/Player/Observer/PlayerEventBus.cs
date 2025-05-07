using UniRx;
using System;
using UnityEngine;

namespace PlayerEvent
{

    public interface IPlayerEventNotifier
    {
        IObservable<Unit> OnAttack { get; }
        IObservable<Unit> OnJump { get; }
        IObservable<Unit> OnDash { get; }
        IObservable<Unit> OnLand { get; }
        IObservable<Unit> OnWallSlideStart { get; }   // ← добавили
    }

    public class PlayerEventBus : IPlayerEventNotifier
    {
        private readonly Subject<Unit> _onAttack = new();
        private readonly Subject<Unit> _onJump = new();
        private readonly Subject<Unit> _onDash = new();
        private readonly Subject<Unit> _onLand = new();
        private readonly Subject<Unit> _onWallSlideStart = new();   // ← новое

        public IObservable<Unit> OnAttack => _onAttack;
        public IObservable<Unit> OnJump => _onJump;
        public IObservable<Unit> OnDash => _onDash;
        public IObservable<Unit> OnLand => _onLand;
        public IObservable<Unit> OnWallSlideStart => _onWallSlideStart;

        // вызовы
        public void RaiseAttack() => _onAttack.OnNext(Unit.Default);
        public void RaiseJump() => _onJump.OnNext(Unit.Default);
        public void RaiseDash() => _onDash.OnNext(Unit.Default);
        public void RaiseLand() => _onLand.OnNext(Unit.Default);
        public void RaiseWallSlideStart() => _onWallSlideStart.OnNext(Unit.Default);  // ←
    }

}