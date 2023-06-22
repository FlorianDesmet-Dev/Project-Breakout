﻿using Microsoft.Xna.Framework;

namespace ProjectBreakout
{
    internal class Ball : Sprite
    {
        float Position_x { get; set; }
        float Position_y { get; set; }

        public Ball(string pNameImage) : base(pNameImage)
        {
            Position_x = ScreenSize.width / 2 - (SpriteTexture.Width / 2);
            Position_y = ScreenSize.height / 2 - (SpriteTexture.Height / 2);
            Position = new Vector2(Position_x, Position_y);
        }

        public override void Load()
        {
            SetPosition(Position);

            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
