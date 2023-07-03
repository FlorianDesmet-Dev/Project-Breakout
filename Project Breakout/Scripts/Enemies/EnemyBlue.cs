﻿using Microsoft.Xna.Framework;

namespace ProjectBreakout;

internal class EnemyBlue : Enemy
{
    public EnemyBlue(string pNameImage) : base(pNameImage)
    {

    }

    public override void Load()
    {
        base.Load();
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
