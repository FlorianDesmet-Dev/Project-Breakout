using Microsoft.Xna.Framework;
using System;

namespace ProjectBreakout;

internal class Ball : Sprite
{
    public enum BallType
    {
        Blue,
        Red,
        Yellow,
        Big
    }

    public Ball(string pNameImage, string pType) : base(pNameImage, pType)
    {
        
    }

    public override void Load()
    {
        base.Load();
    }

    public void ChangeType(BallType pType)
    {
        switch (pType)
        {
            case BallType.Blue:
                Type = "Blue";
                break;
            case BallType.Red:
                Type = "Red";
                break;
            case BallType.Yellow:
                Type = "Yellow";
                break;
            case BallType.Big:
                Type = "Big";
                break;
            default:
                break;
        }
        SpriteTexture = Asset.GetTexture(NameImage + "_" + Type);
    }

    public void StickyBall(Paddle pPaddle)
    {
        SetPosition(
            pPaddle.Position.X + (pPaddle.Width / 2) - (Width / 2),
            pPaddle.Position.Y - Height);

        Speed = new Vector2(2, -2);
    }

    public override void Update(GameTime gameTime)
    {
        BounceLimit();

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}
