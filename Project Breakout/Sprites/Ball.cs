using Microsoft.Xna.Framework;
using static ProjectBreakout.Paddle;
using System;

namespace ProjectBreakout
{
    internal class Ball : Sprite
    {
        public enum BallType
        {
            Green,
            Orange,
            Violet
        }

        public float vx { get; set; }
        public float vy { get; set; }

        public string Type { get; set; }
        public string State { get; set; }

        public Ball(string pType, string pState) : base()
        {
            Type = pType;
            State = pState;
            SpriteTexture = Asset.GetTexture("Ball_" + Type + State);

            vx = 2;
            vy = -2;
            Speed = new Vector2(vx, vy);
        }

        public override void Load()
        {
            base.Load();
        }

        public void ChangeType(BallType pType)
        {
            switch (pType)
            {
                case BallType.Green:
                    Type = "green";
                    break;
                case BallType.Orange:
                    Type = "orange";
                    break;
                case BallType.Violet:
                    Type = "violet";
                    break;
                default:
                    break;
            }
            SpriteTexture = Asset.GetTexture("Ball_" + Type);
        }

        public override void Update(GameTime gameTime)
        {
            Move();

            if (Position.X + Width >= ScreenSize.width)
            {
                Position = new Vector2(ScreenSize.width - Width, Position.Y);
                Speed = new Vector2(-Speed.X, Speed.Y);
            }
            else if (Position.X <= 0)
            {
                Position = new Vector2(0, Position.Y);
                Speed = new Vector2(-Speed.X, Speed.Y);
            }
            else if (Position.Y <= 0)
            {
                Position = new Vector2(Position.X, 0);
                Speed = new Vector2(Speed.X, -Speed.Y);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
