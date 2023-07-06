using Microsoft.Xna.Framework;
using System;

namespace ProjectBreakout;

internal class Brick : Sprite
{
    public enum BrickType
    {
        Blue,
        Red,
        Yellow
    }

    public enum BrickState
    {
        Full,
        TwoBar,
        OneBar
    }

    public Brick(string pNameImage, string pType, string pState) : base(pNameImage, pType, pState)
    {
        Random random = new Random();
        Life = random.Next(1, 4);
    }

    public void ChangeState(BrickState pState)
    {
        switch (pState)
        {
            case BrickState.Full:
                Lenght = "Full";
                break;
            case BrickState.TwoBar:
                Lenght = "2-3";
                break;
            case BrickState.OneBar:
                Lenght = "1-3";
                break;
            default:
                break;
        }
        SpriteTexture = _assets.GetTexture(NameImage + "_" + Color + "_" + Lenght);
    }

    public void ChangeType(BrickType pType)
    {
        switch (pType)
        {
            case BrickType.Blue:
                Color = "Blue";
                break;
            case BrickType.Red:
                Color = "Red";
                break;
            case BrickType.Yellow:
                Color = "Yellow";
                break;
            default:
                break;
        }
        SpriteTexture = _assets.GetTexture(NameImage + "_" + Color + "_" + Lenght);
    }

    public override void Load()
    {
        base.Load();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        switch (Life)
        {
            case 1:
                ChangeState(BrickState.OneBar);
                break;
            case 2:
                ChangeState(BrickState.TwoBar);
                break;
            case 3:
                ChangeState(BrickState.Full);
                break;
            default:
                break;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}
