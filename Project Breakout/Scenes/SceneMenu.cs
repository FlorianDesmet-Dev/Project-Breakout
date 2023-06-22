using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectBreakout
{
    internal class SceneMenu : Scene
    {
        SpriteFont fontMenu;

        public SceneMenu() : base()
        {
            fontMenu = Font.GetFont("FontMenu");
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
            Batch.DrawString(fontMenu,"Menu", Vector2.Zero, Color.White);
            base.Draw(gameTime);
        }
    }
}
