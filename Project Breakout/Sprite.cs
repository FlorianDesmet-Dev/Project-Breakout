using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectBreakout
{
    internal class Sprite
    {
        public IGetAsset Texture { get; private set; }
        public SpriteBatch Batch { get; private set; }
        public IScreenSize ScreenSize { get; private set; }

        public Texture2D SpriteTexture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BoundingBox { get; set; }

        public int Width
        {
            get
            {
                return SpriteTexture.Width;
            }
        }
        public int Height
        {
            get
            { 
                return SpriteTexture.Height; 
            }
        }
        public Sprite(string pNameImage)
        {
            Texture = ServiceLocator.GetService<IGetAsset>();
            Batch = ServiceLocator.GetService<SpriteBatch>();
            ScreenSize = ServiceLocator.GetService<IScreenSize>();

            SpriteTexture = Texture.GetTexture(pNameImage);
        }

        public virtual void Load()
        {

        }

        public void SetPosition(Vector2 pPosition)
        {
            Position = pPosition;
        }

        public virtual void Update(GameTime gameTime)
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public virtual void Draw(GameTime gameTime)
        {
            Batch.Draw(SpriteTexture, Position, Color.White);
        }
    }
}
