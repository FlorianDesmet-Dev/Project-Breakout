using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectBreakout;

internal class SceneMenu : Scene
{
    public SpriteFont TitleFont { get; private set; }
    public Vector2 TitlePosition { get; private set; }
    public Song Intro { get; private set; }

    public SceneMenu() : base()
    {
        TitleFont = _assets.GetFont("TitleFont");

        TitlePosition = new Vector2(
            _screenSize.width / 2 - TitleFont.MeasureString("Press enter").Length() / 2,
            _screenSize.height / 2);

        Intro = _assets.GetSong("Intro");

        MediaPlayer.Play(Intro);
        MediaPlayer.IsRepeating = true;
    }

    public override void Load()
    {
        base.Load();
    }

    public override void Unload()
    {
        MediaPlayer.Stop();
        base.Unload();
    }

    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            _gameState.ChangeScene(GameState.SceneType.Gameplay);
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        _spriteBatch.DrawString(TitleFont, "Press enter", TitlePosition, Color.White);
    }
}
