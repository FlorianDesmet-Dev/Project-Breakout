using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ProjectBreakout;
internal class Ball : Sprite
{
    public enum BallColor
    {
        Blue,
        Red,
        Yellow,
        Big
    }

    public BallColor BType { get; set; }
    public SoundEffect HitSound { get; private set; }


    public Ball(string pNameImage, string pType) : base(pNameImage, pType)
    {
        BType = BallColor.Blue;
        HitSound = _assets.GetSoundEffect("HitSound");
    }

    public override void Load()
    {
        base.Load();
    }

    public void ChangeType(BallColor pType)
    {
        switch (pType)
        {
            case BallColor.Blue:
                Color = "Blue";
                BType = BallColor.Blue;
                break;
            case BallColor.Red:
                Color = "Red";
                BType = BallColor.Red;
                break;
            case BallColor.Yellow:
                Color = "Yellow";
                BType = BallColor.Yellow;
                break;
            case BallColor.Big:
                Color = "Big";
                BType = BallColor.Big;
                break;
            default:
                break;
        }
        SpriteTexture = _assets.GetTexture(NameImage + "_" + Color);
    }

    public void StickyBall(Paddle pPaddle)
    {
        SetPosition(
            pPaddle.Position.X + pPaddle.Width / 2 - Width / 2,
            pPaddle.Position.Y - Height + 4);

        Speed = new Vector2(3, -3);
    }

    public void FastBall()
    {
        float fastSpeed_x;
        float fastSpeed_y;

        if (Speed.X > 0 && Speed.Y > 0)
        {
            fastSpeed_x = Speed.X + 1;
            fastSpeed_y = Speed.Y + 1;
            Speed = new Vector2(fastSpeed_x, fastSpeed_y);
        }
        else if (Speed.X < 0 && Speed.Y < 0)
        {
            fastSpeed_x = Speed.X - 1;
            fastSpeed_y = Speed.Y - 1;
            Speed = new Vector2(fastSpeed_x, fastSpeed_y);
        }
        else if (Speed.X > 0 && Speed.Y < 0)
        {
            fastSpeed_x = Speed.X + 1;
            fastSpeed_y = Speed.Y - 1;
            Speed = new Vector2(fastSpeed_x, fastSpeed_y);
        }
        else if (Speed.X < 0 && Speed.Y > 0)
        {
            fastSpeed_x = Speed.X - 1;
            fastSpeed_y = Speed.Y + 1;
            Speed = new Vector2(fastSpeed_x, fastSpeed_y);
        }
    }

    public void SlowBall()
    {
        float fastSpeed_x;
        float fastSpeed_y;

        if (Speed.X > 0 && Speed.Y > 0)
        {
            fastSpeed_x = Speed.X - 1;
            fastSpeed_y = Speed.Y - 1;
            Speed = new Vector2(fastSpeed_x, fastSpeed_y);
        }
        else if (Speed.X < 0 && Speed.Y < 0)
        {
            fastSpeed_x = Speed.X + 1;
            fastSpeed_y = Speed.Y + 1;
            Speed = new Vector2(fastSpeed_x, fastSpeed_y);
        }
        else if (Speed.X > 0 && Speed.Y < 0)
        {
            fastSpeed_x = Speed.X - 1;
            fastSpeed_y = Speed.Y + 1;
            Speed = new Vector2(fastSpeed_x, fastSpeed_y);
        }
        else if (Speed.X < 0 && Speed.Y > 0)
        {
            fastSpeed_x = Speed.X + 1;
            fastSpeed_y = Speed.Y - 1;
            Speed = new Vector2(fastSpeed_x, fastSpeed_y);
        }
    }

    public void HitX()
    {
        if (!HitSound.Play())
        {
            HitSound.Play();
        }
        ChangeDirectionX();
    }

    public void HitY()
    {
        if (!HitSound.Play())
        {
            HitSound.Play();
        }
        ChangeDirectionY();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        BounceLimit();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}
