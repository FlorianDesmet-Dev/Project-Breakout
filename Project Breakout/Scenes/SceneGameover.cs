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
        TitleFont = Asset.GetFont("TitleFont");
        TextFont = Asset.GetFont("SubTitle");

        GameoverPosition = new Vector2(
            ScreenSize.width / 2 - TitleFont.MeasureString("Game over !").Length() / 2,
            ScreenSize.height / 2);
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
        Batch.DrawString(TitleFont, "Game over !", GameoverPosition, Color.White);
        Batch.DrawString(TextFont, string.Format("Score : {0}", Score), new Vector2(10, 10), Color.White);

        base.Draw(gameTime);
    }
}
