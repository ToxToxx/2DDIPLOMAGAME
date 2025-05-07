// Assets/Game/Scripts/Audio/MusicManager.cs
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        [Header("Music")]
        [SerializeField] private AudioClip _track;
        [SerializeField, Range(0f, 1f)] private float _volume = 0.8f;
        [SerializeField] private bool _loop = true;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.clip = _track;
            _source.volume = _volume;
            _source.loop = _loop;
        }

        private void Start()
        {
            if (_track != null)
                _source.Play();
            else
                Debug.LogWarning("MusicManager: no AudioClip assigned.");
        }
    }
}
