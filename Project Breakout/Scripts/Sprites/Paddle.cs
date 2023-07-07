using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectBreakout;

internal class Paddle : Sprite
{
    public enum PaddleColor
    {
        Blue,
        Red,
        Yellow
    }

    public enum PaddleLenght
    {
        Small,
        Large,
        XXLarge
    }
    
    private Texture2D Heart { get; set; }

    public PaddleColor PColor { get; private set; }
    public PaddleLenght PLenght { get; private set; }

    private KeyboardState NewKeyboardState { get; set; }
    private KeyboardState OldKeyboardState { get; set; }

    public Paddle(string pNameImage, string pType, string pState) : base(pNameImage, pType, pState)
    {
        Heart = _assets.GetTexture("Heart_Full");
        Life = 3;

        Speed = new Vector2(8, 0);

        PLenght = PaddleLenght.Large;
        PColor = PaddleColor.Blue;

        OldKeyboardState = Keyboard.GetState();
    }

    public override void Load()
    {
        SetPosition(
            ScreenSize.width / 2 - SpriteTexture.Width / 2, 
            ScreenSize.height - SpriteTexture.Height * 4);

        base.Load();
    }

    public void ChangeLenght(PaddleLenght pState)
    {
        switch (pState)
        {
            case PaddleLenght.Small:
                Lenght = "Small";
                PLenght = PaddleLenght.Small;
                break;
            case PaddleLenght.Large:
                Lenght = "Large";
                PLenght = PaddleLenght.Large;
                break;
            case PaddleLenght.XXLarge:
                Lenght = "XXLarge";
                PLenght = PaddleLenght.XXLarge;
                break;
            default:
                break;
        }

        SpriteTexture = _assets.GetTexture(NameImage + "_" + Color + "_" + Lenght);
    }

    public void ChangeType(PaddleColor pType)
    {
        switch (pType)
        {
            case PaddleColor.Blue:
                Color = "Blue";
                PColor = PaddleColor.Blue;
                break;
            case PaddleColor.Red:
                Color = "Red";
                PColor = PaddleColor.Red;
                break;
            case PaddleColor.Yellow:
                Color = "Yellow";
                PColor = PaddleColor.Yellow;
                break;
            default:
                break;
        }

        SpriteTexture = _assets.GetTexture(NameImage + "_" + Color + "_" + Lenght);
    }

    public void Commands()
    {
        NewKeyboardState = Keyboard.GetState();

        if (NewKeyboardState.IsKeyDown(Keys.Right))
        {
            Position = new Vector2(Position.X + Speed.X, Position.Y);
        } 
        else if (NewKeyboardState.IsKeyDown(Keys.Left))
        {
            Position = new Vector2(Position.X - Speed.X, Position.Y);
        }

        if (NewKeyboardState.IsKeyDown(Keys.C) &&
            OldKeyboardState != NewKeyboardState)
        {
            switch (Color)
            {
                case "Blue":
                    ChangeType(PaddleColor.Red);
                    break;
                case "Red":
                    ChangeType(PaddleColor.Yellow);
                    break;
                case "Yellow":
                    ChangeType(PaddleColor.Blue);
                    break;
                default:
                    break;
            }
        }

        OldKeyboardState = NewKeyboardState;
    }

    public void InvertedCommands()
    {
        NewKeyboardState = Keyboard.GetState();

        if (NewKeyboardState.IsKeyDown(Keys.Right))
        {
            Position = new Vector2(Position.X - Speed.X, Position.Y);
        }
        else if (NewKeyboardState.IsKeyDown(Keys.Left))
        {
            Position = new Vector2(Position.X + Speed.X, Position.Y);
        }

        if (NewKeyboardState.IsKeyDown(Keys.C) &&
            OldKeyboardState != NewKeyboardState)
        {
            switch (Color)
            {
                case "Blue":
                    ChangeType(PaddleColor.Red);
                    break;
                case "Red":
                    ChangeType(PaddleColor.Yellow);
                    break;
                case "Yellow":
                    ChangeType(PaddleColor.Blue);
                    break;
                default:
                    break;
            }
        }

        OldKeyboardState = NewKeyboardState;
    }

    public override void Update(GameTime gameTime)
    {
        BoundingBox = new Rectangle((int)Position.X, (int)Position.Y + 4, Width, Height - 4);

        if (Position.X + Width >= ScreenSize.width)
        {
            Position = new Vector2(ScreenSize.width - Width, Position.Y);
        }
        else if (Position.X <= 0)
        {
            Position = new Vector2(0, Position.Y);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        int offset_x = 0;

        for (int i = 1; i <= Life; i++)
        {
            _spriteBatch.Draw(
                Heart, 
                new Vector2(ScreenSize.width - Heart.Width - offset_x - 10, ScreenSize.height - Heart.Height - 10), 
                Microsoft.Xna.Framework.Color.White);
            
            offset_x += Heart.Width;
        }
    }
}
