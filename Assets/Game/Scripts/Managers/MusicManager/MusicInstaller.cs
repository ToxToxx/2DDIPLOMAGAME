using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicInstaller : MonoInstaller
    {
        [Header("Music Settings")]
        [SerializeField] private AudioClip _track;
        [SerializeField, Range(0f, 1f)] private float _volume = 0.8f;
        [SerializeField] private bool _loop = true;

        [Header("UI")]
        [SerializeField] private Button _toggleMusicButton;

        public override void InstallBindings()
        {
            var src = GetComponent<AudioSource>();
            src.clip = _track;
            src.volume = _volume;
            src.loop = _loop;
            src.playOnAwake = false;
            src.Play();

            Container.Bind<AudioSource>()
                     .FromInstance(src)
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<MusicPlayer>()
                     .AsSingle()
                     .WithArguments(_track, _volume, _loop, _toggleMusicButton);
        }
    }
}
