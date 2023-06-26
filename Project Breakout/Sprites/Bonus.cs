using Microsoft.Xna.Framework;

namespace ProjectBreakout
{
    internal class Bonus : Sprite
    {
        public float vx { get; set; }
        public float vy { get; set; }

        public Bonus(string pNameImage) : base(pNameImage)
        {
            vx = 0;
            vy = 2;
            Speed = new Vector2(vx, vy);
        }

        public override void Load()
        {
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            Move();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}
