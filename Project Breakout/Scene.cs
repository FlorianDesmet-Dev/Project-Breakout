using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ProjectBreakout
{
    public class Scene
    {
        public SpriteBatch Batch { get; private set; }
        public IGetAsset Asset { get; private set; }
        public IScreenSize ScreenSize { get; private set; }
        public GameState GameState { get; set; }

        public Scene()
        {
            Batch = ServiceLocator.GetService<SpriteBatch>();
            Asset = ServiceLocator.GetService<IGetAsset>();
            ScreenSize = ServiceLocator.GetService<IScreenSize>();
            GameState = ServiceLocator.GetService<GameState>();
        }

        public virtual void Load()
        {

        }

        public virtual void Unload()
        {
            Debug.WriteLine("Unload");
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {

        }
    }
}
