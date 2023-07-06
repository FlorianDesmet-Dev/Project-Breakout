using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectBreakout;

internal class Projectile : Sprite
{
    public float Angle { get; private set; }
    public Projectile(string pNameImage, float pAngle) : base(pNameImage)
    {
        Angle = MathHelper.ToRadians(pAngle);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Draw(
            SpriteTexture, 
            Position, 
            null, 
            Microsoft.Xna.Framework.Color.White, 
            Angle, 
            Vector2.Zero, 
            1.0f,
            SpriteEffects.None, 
            0);
    }
}
