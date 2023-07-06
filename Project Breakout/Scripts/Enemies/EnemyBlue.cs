using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ProjectBreakout;

internal class EnemyBlue : Enemy
{
    public List<Projectile> ListProjectiles { get; private set; }
    private float TimerFire { get; set; }

    public EnemyBlue(string pNameImage) : base(pNameImage)
    {
        TimerFire = 5f;

        ListProjectiles = new List<Projectile>();
    }

    public override void Load()
    {
        base.Load();
    }

    public override void Update(GameTime gameTime)
    {
        BounceLimit();
        base.Update(gameTime);

        if (EState != EnemyState.Appear)
        {
            TimerFire -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TimerFire <= 0)
            {
                Projectile projectile = new("Bullet", 180);
                projectile.SetPosition(
                    Position.X + (Width / 2) + (projectile.Width / 2),
                    Position.Y + Height);
                
                ListProjectiles.Add(projectile);

                TimerFire = 5f;
            }

            for (int i = ListProjectiles.Count - 1; i  >= 0; i--)
            {
                Projectile projectile = ListProjectiles[i];
                projectile.Move(0f, 4f);
                projectile.Update(gameTime);

                if (projectile.Position.Y >= ScreenSize.height)
                {
                    ListProjectiles.Remove(projectile);
                }
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        foreach (Projectile projectile in ListProjectiles)
        {
            projectile.Draw(gameTime);
        }
    }
}
