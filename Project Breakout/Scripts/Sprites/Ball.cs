using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

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

    public float Angle { get; set; }
    public new float Speed { get; set; }
    public Vector2 Direction { get; set; }

    public BallColor BType { get; set; }
    public SoundEffect HitSound { get; private set; }


    public Ball(string pNameImage, string pType) : base(pNameImage, pType)
    {
        BType = BallColor.Blue;
        HitSound = _assets.GetSoundEffect("HitSound");

        Angle = MathHelper.ToRadians(-45);
        Speed = 4;
        Direction = Vector2.Zero;
    }

    public override void Load()
    {
        base.Load();
    }

    public void ChangeColor(BallColor pType)
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

    public void StickyBall(float pX, float pY, int pWidth)
    {
        SetPosition(
            pX + pWidth / 2 - Width / 2,
            pY - Height + 4);

        // Speed = new Vector2(3, -3);

        Direction = new Vector2(
            Speed * (float)Math.Cos(Angle),
            Speed * (float)Math.Sin(Angle));
    }

    /* public void FastBall()
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

    public void RestoreSpeed(float pX, float pY)
    {
        if (Speed.X > 0 && Speed.Y > 0)
        {
            Speed = new Vector2(pX, pY);
        }
        else if (Speed.X < 0 && Speed.Y < 0)
        {
            Speed = new Vector2(-pX, -pY);
        }
        else if (Speed.X > 0 && Speed.Y < 0)
        {
            Speed = new Vector2(pX, -pY);
        }
        else if (Speed.X < 0 && Speed.Y > 0)
        {
            Speed = new Vector2(-pX, pY);
        }
    } */

    public new Rectangle NextPositionX()
    {
        Rectangle nextPosition = BoundingBox;
        nextPosition.Offset(new Point((int)Direction.X, 0));
        return nextPosition;
    }

    public new Rectangle NextPositionY()
    {
        Rectangle nextPosition = BoundingBox;
        nextPosition.Offset(new Point(0, (int)Direction.Y));
        return nextPosition;
    }

    public override void ChangeDirectionX()
    {
        Direction = new Vector2(-Direction.X, Direction.Y);
    }

    public override void ChangeDirectionY()
    {
        Direction = new Vector2(Direction.X, -Direction.Y);
    }

    public override void BounceLimit()
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
        BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        Position += Direction;
        // base.Update(gameTime);
        BounceLimit();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}
