using UniRx;
using Zenject;
using PlayerEvent;
using System;
using UnityEngine;

namespace PlayerAudio
{
    public class PlayerAudioController : IInitializable, IDisposable
    {
        private readonly AudioSource _source;
        private readonly PlayerAudioConfig _cfg;
        private readonly IPlayerEventNotifier _events;
        private readonly CompositeDisposable _disposables = new();

        public PlayerAudioController(AudioSource source,
                                     PlayerAudioConfig cfg,
                                     IPlayerEventNotifier events)
        {
            _source = source;
            _cfg = cfg;
            _events = events;
        }

        public void Initialize()
        {
            _source.playOnAwake = false;

            _events.OnJump.Subscribe(_ => Play(_cfg.Jump)).AddTo(_disposables);
            _events.OnLand.Subscribe(_ => Play(_cfg.Land)).AddTo(_disposables);
            _events.OnDash.Subscribe(_ => Play(_cfg.Dash)).AddTo(_disposables);
            _events.OnAttack.Subscribe(_ => Play(_cfg.Attack)).AddTo(_disposables);
            _events.OnWallSlideStart.Subscribe(_ => Play(_cfg.Land)).AddTo(_disposables); 
        }


        private void Play(AudioClip clip)
        {
            if (clip != null) _source.PlayOneShot(clip, _cfg.Volume);
        }

        public void Dispose() => _disposables.Dispose();
    }
}
