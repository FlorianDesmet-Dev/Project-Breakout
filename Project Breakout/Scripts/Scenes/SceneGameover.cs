using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace ProjectBreakout;

internal class SceneGameover : Scene
{
    public SpriteFont TitleFont { get; private set; }
    public SpriteFont TextFont { get; private set; }
    public Vector2 GameoverPosition { get; private set; }
    public Vector2 ScorePosition { get; private set; }

    public int Score { get; private set; }

    public SceneGameover() : base()
    {
        TitleFont = _assets.GetFont("TitleFont");
        TextFont = _assets.GetFont("SubTitle");

        GameoverPosition = new Vector2(
            _screenSize.width / 2 - TitleFont.MeasureString("Game over !").Length() / 2,
            _screenSize.height / 2);
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

        _spriteBatch.DrawString(TitleFont, "Game over !", GameoverPosition, Color.White);
        _spriteBatch.DrawString(TextFont, string.Format("Score : {0}", Score), new Vector2(10, 10), Color.White);
    }
}
