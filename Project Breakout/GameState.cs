namespace ProjectBreakout;

internal class GameState
{
    public enum SceneType
    {
        Menu,
        Gameplay,
        Gameover
    }

    public Scene CurrentScene { get; private set; }

    public GameState()
    {

    }

    public void ChangeScene(SceneType pSceneType)
    {
        if (CurrentScene != null)
        {
            CurrentScene.Unload();
            CurrentScene = null;
        }

        switch (pSceneType)
        {
            case SceneType.Menu:
                CurrentScene = new SceneMenu();
                break;
            case SceneType.Gameplay:
                CurrentScene = new SceneGameplay();
                break;
            case SceneType.Gameover:
                CurrentScene = new SceneGameover();
                break;
            default:
                break;
        }

        CurrentScene.Load();
    }
}
