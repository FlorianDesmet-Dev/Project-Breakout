using Microsoft.Xna.Framework.Graphics;

namespace ProjectBreakout;

internal class ListBonus
{
    public SpriteBatch _SpriteBatch { get; private set; }

    public ListBonus()
    {
        _SpriteBatch = ServiceLocator.GetService<SpriteBatch>();
    }
}
