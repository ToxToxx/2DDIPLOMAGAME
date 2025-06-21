using Game.UI;
using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenu;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PauseController>()
                 .AsSingle()
                 .WithArguments(_pauseMenu)
                 .NonLazy();
    }
}
