using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectBreakout
{
    internal class SceneGameover : Scene
    {
        public SpriteFont FontGameover { get; private set; }
        public Vector2 FontPosition { get; private set; }

        public SceneGameover() : base()
        {
            FontGameover = Asset.GetFont("FontMenu");
            FontPosition = new Vector2(
                ScreenSize.width / 2 - FontGameover.MeasureString("Game over !").Length() / 2,
                ScreenSize.height / 2);
        }

        public override void Load()
        {
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
            Batch.DrawString(FontGameover, "Game over !", FontPosition, Color.White);
            base.Draw(gameTime);
        }
    }
}
