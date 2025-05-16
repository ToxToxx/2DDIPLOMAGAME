using UnityEngine;
using Zenject;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicInstaller : MonoInstaller
    {
        [Header("Music Settings")]
        [SerializeField] private AudioClip _track;
        [SerializeField, Range(0f, 1f)] private float _volume = 0.8f;
        [SerializeField] private bool _loop = true;

        public override void InstallBindings()
        {
            var src = GetComponent<AudioSource>();

            Container.Bind<AudioSource>().FromInstance(src).AsSingle();

            Container.BindInterfacesAndSelfTo<MusicPlayer>()
                     .AsSingle()
                     .WithArguments(_track, _volume, _loop);  
        }
    }
}
