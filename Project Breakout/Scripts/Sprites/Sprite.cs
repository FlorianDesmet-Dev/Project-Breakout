using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectBreakout;

internal abstract class Sprite
{
    protected IGetAssets _assets { get; private set; }
    protected SpriteBatch _spriteBatch { get; private set; }
    protected IScreenSize ScreenSize { get; private set; }

    public Texture2D SpriteTexture { get; protected set; }
    public Vector2 Position { get; set; }
    public Vector2 Speed { get; set; }
    public Rectangle BoundingBox { get; protected set; }

    public string NameImage { get; set; }
    public int Life { get; set; }

    public int Width { get { return SpriteTexture.Width; } }
    public int Height { get { return SpriteTexture.Height; } }

    public string Color { get; protected set; }
    public string Lenght { get; protected set; }

    public Sprite(string pNameImage)
    {
        _assets = ServiceLocator.GetService<IGetAssets>();
        _spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        ScreenSize = ServiceLocator.GetService<IScreenSize>();

        NameImage = pNameImage;
        SpriteTexture = _assets.GetTexture(NameImage);
    }

    public Sprite(string pNameImage, string pColor)
    {
        _assets = ServiceLocator.GetService<IGetAssets>();
        _spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        ScreenSize = ServiceLocator.GetService<IScreenSize>();

        NameImage = pNameImage;
        Color = pColor;
        SpriteTexture = _assets.GetTexture(pNameImage + "_" + Color);
    }

    public Sprite(string pNameImage, string pColor, string pLenght)
    {
        _assets = ServiceLocator.GetService<IGetAssets>();
        _spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        ScreenSize = ServiceLocator.GetService<IScreenSize>();

        NameImage = pNameImage;
        Color = pColor;
        Lenght = pLenght;
        SpriteTexture = _assets.GetTexture(NameImage + "_" + Color + "_" + Lenght);
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

    public virtual void Move(float pX, float pY)
    {
        Position = new Vector2(Position.X + pX, Position.Y + pY);
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
        _spriteBatch.Draw(SpriteTexture, Position, Microsoft.Xna.Framework.Color.White);
    }
}
