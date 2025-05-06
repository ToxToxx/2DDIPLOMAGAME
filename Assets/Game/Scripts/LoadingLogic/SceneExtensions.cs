using System.Reflection;

namespace Game
{
    [ObfuscationAttribute(Exclude = true)]
    public static class SceneExtensions
    {
        public static string GetSceneName(Loader.Scene scene)
        {
            switch (scene)
            {
                case Loader.Scene.MainMenu: return "MainMenu";
                case Loader.Scene.LoadingScreen: return "LoadingScreen";
                case Loader.Scene.GameScene: return "GameScene";
                default: return string.Empty;
            }
        }
    }
}

