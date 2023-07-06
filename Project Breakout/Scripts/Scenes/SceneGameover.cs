using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace ProjectBreakout;

internal class SceneGameover : Scene
{
    public Vector2 ScorePosition { get; private set; }

    public int Score { get; private set; }

    public SceneGameover() : base()
    {
        TitleFont = _assets.GetFont("Title");

        SizeFont = TitleFont.MeasureString("GAMEOVER");

        TitlePosition = new Vector2(
            _screenSize.width / 2 - SizeFont.X / 2,
            _screenSize.height / 2 - SizeFont.Y / 2);

        ShadePosition = new Vector2(
            _screenSize.width / 2 - SizeFont.X / 2 + 5,
            _screenSize.height / 2 - SizeFont.Y / 2 + 5);
    }

    public override void Load()
    {
        // Load Last Score
        Score = ScoreManager.LoadScore();

        base.Load();
    }

    public override void Unload()
    {
        base.Unload();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _spriteBatch.DrawString(TitleFont, "GAMEOVER", ShadePosition, Color.DarkRed);
        _spriteBatch.DrawString(TitleFont, "GAMEOVER", TitlePosition, Color.White);
    }
}
