using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [ObfuscationAttribute(Exclude = true)]
    public class ChangeSceneButton : MonoBehaviour
    {
        [SerializeField] private Button _changeSceneButton;
        [SerializeField] private Loader.Scene _targetScene;

        private void Awake()
        {
            _changeSceneButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                Loader.Load(_targetScene);
            });
        }

    }
}

