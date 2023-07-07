using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;

namespace ProjectBreakout;

internal class SceneGameover : Scene
{
    private SpriteFont ScoreFont {  get; set; }
    private Vector2 ScorePosition { get; set; }
    
    private Song GameOver { get; set; }
    public int Score { get; private set; }

    public SceneGameover() : base()
    {
        TitleFont = _assets.GetFont("Title");
        SizeFont = TitleFont.MeasureString("GAMEOVER");

        TitlePosition = new Vector2(
            _screenSize.width / 2 - SizeFont.X / 2,
            (_screenSize.height / 2) - SizeFont.Y);

        ShadePosition = new Vector2(
            _screenSize.width / 2 - SizeFont.X / 2 + 5,
            (_screenSize.height / 2) - SizeFont.Y + 5);

        ScoreFont = _assets.GetFont("SubTitle");
        SizeFont = ScoreFont.MeasureString(string.Format("Last Score : {0}", Score));

        ScorePosition = new Vector2(
            0 + 10,
            _screenSize.height - SizeFont.Y - 10);

        StartButton = new Button("ButtonRestart");
        StartButton.Position = new Vector2(
            _screenSize.width / 2 - StartButton.Width / 2,
            (_screenSize.height / 2) + (StartButton.Height / 2));
    }

    public override void Load()
    {
        Score = ScoreManager.LoadScore();

        GameOver = _assets.GetSong("sky-lines");
        MediaPlayer.Play(GameOver);
        MediaPlayer.IsRepeating = true;

        StartButton.OnClick = onClickPlay;

        base.Load();
    }

    public override void Unload()
    {
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
        _spriteBatch.DrawString(TitleFont, "GAMEOVER", ShadePosition, Color.DarkRed);
        _spriteBatch.DrawString(TitleFont, "GAMEOVER", TitlePosition, Color.White);
        _spriteBatch.DrawString(ScoreFont, string.Format("Last Score : {0}", Score), ScorePosition, Color.White);
    }
}
