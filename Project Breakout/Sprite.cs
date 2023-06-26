using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectBreakout
{
    internal abstract class Sprite
    {
        public IGetAsset Asset { get; private set; }
        public SpriteBatch Batch { get; private set; }
        public IScreenSize ScreenSize { get; private set; }

        public Texture2D SpriteTexture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BoundingBox { get; set; }
        public Vector2 Speed { get; set; }

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
            Asset = ServiceLocator.GetService<IGetAsset>();
            Batch = ServiceLocator.GetService<SpriteBatch>();
            ScreenSize = ServiceLocator.GetService<IScreenSize>();

            SpriteTexture = Asset.GetTexture(pNameImage);
        }

        public Sprite()
        {
            Asset = ServiceLocator.GetService<IGetAsset>();
            Batch = ServiceLocator.GetService<SpriteBatch>();
            ScreenSize = ServiceLocator.GetService<IScreenSize>();
        }

        public virtual void Load()
        {

        }

        public void SetPosition(float pX, float pY)
        {
            Position = new Vector2(pX, pY);
        }

        public Rectangle NextPositionX()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point((int)Speed.X, 0));
            return nextPosition;
        }

        public Rectangle NextPositionY()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point(0, (int)Speed.Y));
            return nextPosition;
        }

        public virtual void Move()
        {
            Position += Speed;
        }

        public virtual void ChangeDirectionX()
        {
            Speed = new Vector2(-Speed.X, Speed.Y);
        }

        public virtual void ChangeDirectionY()
        {
            Speed = new Vector2(Speed.X, -Speed.Y);
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
