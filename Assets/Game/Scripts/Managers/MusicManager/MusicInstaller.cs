// Assets/Game/Scripts/Audio/MusicInstaller.cs
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

            // Делаем AudioSource single‑instance, чтобы другие сервисы могли запросить при желании
            Container.Bind<AudioSource>().FromInstance(src).AsSingle();

            // Передаём настройки в сервис
            Container.BindInterfacesAndSelfTo<MusicPlayer>()
                     .AsSingle()
                     .WithArguments(_track, _volume, _loop);   // AudioSource придёт из контейнера
        }
    }
}
