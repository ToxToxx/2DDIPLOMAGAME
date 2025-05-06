using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [ObfuscationAttribute(Exclude = true)]
    public class Loader : MonoBehaviour
    {
        public enum Scene
        {
            MainMenu,
            LoadingScreen,
            GameScene,
        }

        private static Scene _targetScene;

        public static void Load(Scene target)
        {
            _targetScene = target;
            SceneManager.LoadScene(Scene.LoadingScreen.ToString());
        }

        public static void LoaderCallback()
        {
            SceneManager.LoadScene(_targetScene.ToString());
        }

    }
}
