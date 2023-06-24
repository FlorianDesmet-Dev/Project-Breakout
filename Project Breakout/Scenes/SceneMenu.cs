using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectBreakout
{
    internal class SceneMenu : Scene
    {
        public SpriteFont FontMenu { get; private set; }
        public Vector2 FontPosition { get; private set; }

        public SceneMenu() : base()
        {
            FontMenu = Asset.GetFont("FontMenu");
            FontPosition = new Vector2(
                ScreenSize.width / 2 - FontMenu.MeasureString("Press enter").Length() / 2, 
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
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameState.ChangeScene(GameState.SceneType.Gameplay);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Batch.DrawString(FontMenu,"Press enter", FontPosition, Color.White);
            base.Draw(gameTime);
        }
    }
}
