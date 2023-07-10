using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectBreakout;

internal class Background
{
    private SpriteBatch _spriteBatch { get; set; }
    private IGetAssets _assets { get; set; }
    protected IScreenSize _screenSize { get; private set; }

    private float Speed { get; set; }
    private Vector2 Position { get; set; }
    private Texture2D BackgroundTexture { get; set; }
    private string NameImage { get; set; }

    public Background(string pNameImage, float pSpeed)
    {
        _spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        _assets = ServiceLocator.GetService<IGetAssets>();
        _screenSize = ServiceLocator.GetService<IScreenSize>();

        NameImage = pNameImage;
        Speed = pSpeed;
        BackgroundTexture = _assets.GetTexture(NameImage);

        Position = new Vector2(0, 0);
    }

    public void Update(GameTime gameTime)
    {
        Position = new Vector2(Position.X + Speed, Position.Y);
        if (Position.X <= 0 - BackgroundTexture.Width)
        {
            Position = new Vector2(0, Position.Y);
        }
    }

    public void Draw(GameTime gameTime)
    {
        _spriteBatch.Draw(BackgroundTexture, Position, Color.White);

        if (Position.X <= 0)
        {
            _spriteBatch.Draw(BackgroundTexture, new Vector2(Position.X + BackgroundTexture.Width, 0), Color.White);
        }
    }
}
