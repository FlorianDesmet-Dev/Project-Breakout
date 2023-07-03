using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectBreakout;

internal abstract class Sprite
{
    protected IGetAssets Asset { get; private set; }
    protected SpriteBatch _spriteBatch { get; private set; }
    protected IScreenSize ScreenSize { get; private set; }

    public Texture2D SpriteTexture { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Speed { get; set; }
    public Rectangle BoundingBox { get; set; }

    public string NameImage { get; set; }
    public int Life { get; set; }

    public int Width { get { return SpriteTexture.Width; } }
    public int Height { get { return SpriteTexture.Height; } }

    public string Type { get; protected set; }
    public string State { get; protected set; }

    public Sprite(string pNameImage)
    {
        Asset = ServiceLocator.GetService<IGetAssets>();
        _spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        ScreenSize = ServiceLocator.GetService<IScreenSize>();
        
        NameImage = pNameImage;
        SpriteTexture = Asset.GetTexture(NameImage);
    }

    public Sprite(string pNameImage, string pType)
    {
        Asset = ServiceLocator.GetService<IGetAssets>();
        _spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        ScreenSize = ServiceLocator.GetService<IScreenSize>();

        NameImage = pNameImage;
        Type = pType;
        SpriteTexture = Asset.GetTexture(pNameImage + "_" + Type);
    }

    public Sprite(string pNameImage, string pType, string pState)
    {
        Asset = ServiceLocator.GetService<IGetAssets>();
        _spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        ScreenSize = ServiceLocator.GetService<IScreenSize>();

        NameImage = pNameImage;
        Type = pType;
        State = pState;
        SpriteTexture = Asset.GetTexture(NameImage + "_" + Type + "_" + State);
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

    public virtual void ChangeDirectionX()
    {
        Speed = new Vector2(-Speed.X, Speed.Y);
    }

    public virtual void ChangeDirectionY()
    {
        Speed = new Vector2(Speed.X, -Speed.Y);
    }

    public virtual void Move()
    {
        Position += Speed;
    }
    
    public virtual void BounceLimit()
    {
        if (Position.X + Width >= ScreenSize.width)
        {
            Position = new Vector2(ScreenSize.width - Width, Position.Y);
            ChangeDirectionX();
        }
        else if (Position.X <= 0)
        {
            Position = new Vector2(0, Position.Y);
            ChangeDirectionX();
        }
        else if (Position.Y <= 0)
        {
            Position = new Vector2(Position.X, 0);
            ChangeDirectionY();
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
    }

    public virtual void Draw(GameTime gameTime)
    {
        _spriteBatch.Draw(SpriteTexture, Position, Color.White);
    }
}
