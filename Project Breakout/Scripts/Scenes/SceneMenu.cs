using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace ProjectBreakout;

internal class SceneMenu : Scene
{
    private Button StartButton { get; set; }
    private Song Intro { get; set; }

    public SceneMenu() : base()
    {
        TitleFont = _assets.GetFont("Title");

        SizeFont = TitleFont.MeasureString("COLORBREAKER");

        TitlePosition = new Vector2(
            _screenSize.width / 2 - SizeFont.X / 2,
            (_screenSize.height / 3) - SizeFont.Y / 2);

        ShadePosition = new Vector2(
            _screenSize.width / 2 - SizeFont.X / 2 + 5,
            (_screenSize.height / 3) - SizeFont.Y / 2 + 5);

        StartButton = new Button("Button");
        StartButton.Position = new Vector2(
            _screenSize.width / 2 - StartButton.Width / 2,
            (_screenSize.height / 3) - (StartButton.Height / 2) + SizeFont.Y);

        StartButton.OnClick = onClickPlay;

        Intro = _assets.GetSong("Intro");

        MediaPlayer.Play(Intro);
        MediaPlayer.IsRepeating = true;
    }

    public void onClickPlay(Button pSender)
    {
        _gameState.ChangeScene(GameState.SceneType.Gameplay);
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

        StartButton.Update(gameTime);

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {           
        base.Draw(gameTime);

        _spriteBatch.Draw(StartButton.SpriteTexture, StartButton.Position, Color.White);

        _spriteBatch.DrawString(TitleFont, "COLORBREAKER", ShadePosition, Color.Red);
        _spriteBatch.DrawString(TitleFont, "COLORBREAKER", TitlePosition, Color.White);
    }
}
