using Microsoft.Xna.Framework;

namespace ProjectBreakout
{
    internal class Ball : Sprite
    {
        public float vx { get; set; }
        public float vy { get; set; }

        public Ball(string pNameImage) : base(pNameImage)
        {
            vx = 2;
            vy = -2;
            Speed = new Vector2(vx, vy);

        }

        public override void Load()
        {
            base.Load();
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
