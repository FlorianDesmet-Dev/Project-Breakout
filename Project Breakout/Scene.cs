using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectBreakout
{
    public class Scene
    {
        public SpriteBatch Batch { get; private set; }
        public IGetAsset Font { get; set; }
        public IGetAsset Texture { get; set; }

        public Scene()
        {
            Batch = ServiceLocator.GetService<SpriteBatch>();
            Font = ServiceLocator.GetService<IGetAsset>();
        }

        public virtual void Load()
        {

        }

        public virtual void Unload()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {

        }
    }
}
