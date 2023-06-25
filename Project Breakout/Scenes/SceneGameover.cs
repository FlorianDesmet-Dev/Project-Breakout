using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectBreakout
{
    internal class SceneGameover : Scene
    {
        public SpriteFont FontGameover { get; private set; }
        public Vector2 GameoverPosition { get; private set; }
        public Vector2 ScorePosition { get; private set; }

        public Size sizeGameover { get; private set; }
        public Size sizeScore { get; private set; }

        public int Score { get; private set; }

        public SceneGameover() : base()
        {
            FontGameover = Asset.GetFont("FontMenu");
        }

        public override void Load()
        {
            // Load Last Score
            Score = ScoreManager.LoadScore();

            sizeGameover = TextRenderer.MeasureText("Game over", new Font("FontMenu", 14, FontStyle.Bold));
            sizeScore = TextRenderer.MeasureText(string.Format("Score : {0}", Score), new Font("FontMenu", 14, FontStyle.Bold));

            GameoverPosition = new Vector2(
                (ScreenSize.width / 2) - (sizeGameover.Width / 2),
                (ScreenSize.height / 2) - (sizeGameover.Height / 2));

            ScorePosition = new Vector2(
                (ScreenSize.width / 2) - (sizeScore.Width / 2),
                (ScreenSize.height / 2) + sizeGameover.Height);

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
            Batch.DrawString(FontGameover, "Game over", GameoverPosition, Color.White);
            Batch.DrawString(FontGameover, string.Format("Score : {0}", Score), ScorePosition, Color.White);

            base.Draw(gameTime);
        }
    }
}
