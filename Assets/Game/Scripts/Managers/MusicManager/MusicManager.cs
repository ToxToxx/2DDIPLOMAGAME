using UnityEngine;
using Zenject;

namespace Audio
{
    public class MusicPlayer : IInitializable
    {
        private readonly AudioSource _source;
        private readonly AudioClip _clip;
        private readonly float _volume;
        private readonly bool _loop;

        public MusicPlayer(AudioSource source, AudioClip clip, float volume, bool loop)
        {
            _source = source;
            _clip = clip;
            _volume = volume;
            _loop = loop;
        }

        public void Initialize()
        {
            if (_clip == null)
            {
                Debug.LogWarning("MusicPlayer: no AudioClip assigned.");
                return;
            }

            _source.playOnAwake = false;
            _source.clip = _clip;
            _source.volume = _volume;
            _source.loop = _loop;
            _source.Play();
        }
    }
}
