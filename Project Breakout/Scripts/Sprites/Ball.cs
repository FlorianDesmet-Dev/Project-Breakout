using Microsoft.Xna.Framework;

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

    public BallType BT { get; set; }

    public Ball(string pNameImage, string pType) : base(pNameImage, pType)
    {
        BT = BallType.Blue;
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
                BT = BallType.Blue;
                break;
            case BallType.Red:
                Type = "Red";
                BT = BallType.Red;
                break;
            case BallType.Yellow:
                Type = "Yellow";
                BT = BallType.Yellow;
                break;
            case BallType.Big:
                Type = "Big";
                BT = BallType.Big;
                break;
            default:
                break;
        }
        SpriteTexture = Asset.GetTexture(NameImage + "_" + Type);
    }

    public void StickyBall(Paddle pPaddle)
    {
        SetPosition(
            pPaddle.Position.X + pPaddle.Width / 2 - Width / 2,
            pPaddle.Position.Y - Height);

        Speed = new Vector2(3, -3);
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
