using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Audio
{
    public class MusicPlayer : IInitializable, IDisposable
    {
        private readonly AudioSource _source;
        private readonly Button _toggleButton;
        private readonly Color _onColor = Color.white;
        private readonly Color _offColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        private bool _isMuted;

        public MusicPlayer(
            AudioClip track,
            float volume,
            bool loop,
            Button toggleButton,
            AudioSource source)
        {
            _source = source;
            _toggleButton = toggleButton;
        }

        public void Initialize()
        {
            _toggleButton.onClick.AddListener(ToggleMusic);
            UpdateButtonVisual();
        }

        private void ToggleMusic()
        {
            _isMuted = !_isMuted;
            _source.mute = _isMuted;
            UpdateButtonVisual();
        }

        private void UpdateButtonVisual()
        {
            var img = _toggleButton.GetComponent<Image>();
            if (img != null)
                img.color = _isMuted ? _offColor : _onColor;
        }

        public void Dispose()
        {
            _toggleButton.onClick.RemoveListener(ToggleMusic);
        }
    }
}
