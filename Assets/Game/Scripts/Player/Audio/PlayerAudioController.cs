using UnityEngine;
using UniRx;
using Zenject;
using PlayerEvent;
using System;

namespace PlayerAudio
{
    public class PlayerAudioController : MonoBehaviour, IInitializable, IDisposable
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _jumpClip;
        [SerializeField] private AudioClip _landClip;
        [SerializeField] private AudioClip _dashClip;
        [SerializeField] private AudioClip _attackClip;
        [SerializeField] private float _volume = 1f;

        private AudioSource _source;
        private readonly CompositeDisposable _disposables = new();

        // ➊  Поле‑инъекция — Zenject заполнит ссылку даже для FromInstance
        [Inject] private IPlayerEventNotifier _events = null;

        public void Initialize()
        {
            _source = gameObject.AddComponent<AudioSource>();
            _source.playOnAwake = false;

            _events.OnJump.Subscribe(_ => Play(_jumpClip)).AddTo(_disposables);
            _events.OnLand.Subscribe(_ => Play(_landClip)).AddTo(_disposables);
            _events.OnDash.Subscribe(_ => Play(_dashClip)).AddTo(_disposables);
            _events.OnAttack.Subscribe(_ => Play(_attackClip)).AddTo(_disposables);
        }

        private void Play(AudioClip clip)
        {
            if (clip != null) _source.PlayOneShot(clip, _volume);
        }

        public void Dispose() => _disposables.Dispose();
    }
}
