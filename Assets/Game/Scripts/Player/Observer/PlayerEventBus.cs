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
        IObservable<Unit> OnInteract { get; }
    }

    public class PlayerEventBus : IPlayerEventNotifier
    {
        private readonly Subject<Unit> _onAttack = new Subject<Unit>();
        private readonly Subject<Unit> _onJump = new Subject<Unit>();
        private readonly Subject<Unit> _onDash = new Subject<Unit>();
        private readonly Subject<Unit> _onLand = new Subject<Unit>();
        private readonly Subject<Unit> _onInteract = new Subject<Unit>();

        public IObservable<Unit> OnAttack => _onAttack;
        public IObservable<Unit> OnJump => _onJump;
        public IObservable<Unit> OnDash => _onDash;
        public IObservable<Unit> OnLand => _onLand;
        public IObservable<Unit> OnInteract => _onInteract;

        public void RaiseAttack()
        {
            _onAttack.OnNext(Unit.Default);
        }

        public void RaiseJump()
        {
            _onJump.OnNext(Unit.Default);
        }

        public void RaiseDash()
        {
            _onDash.OnNext(Unit.Default);
        }

        public void RaiseLand()
        {
            _onLand.OnNext(Unit.Default);
        }

        public void RaiseInteract()
        {
            _onInteract.OnNext(Unit.Default);
        }
    }
}