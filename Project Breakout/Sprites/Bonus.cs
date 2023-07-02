using Microsoft.Xna.Framework;
using System;

namespace ProjectBreakout;

internal class Bonus : Sprite
{
    public Bonus(string pNameImage) : base(pNameImage)
    {
        Speed = new Vector2(0, 2);
    }

    public override void Load()
    {
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
