using Microsoft.Xna.Framework;

public interface IScreenSize
{
    int width { get; }
    int height { get; }
}

internal class ScreenManager : IScreenSize
{
    private GraphicsDeviceManager _graphics;
    public int width { get { return _graphics.PreferredBackBufferWidth; } }
    public int height { get { return _graphics.PreferredBackBufferHeight; } }

    public ScreenManager(GraphicsDeviceManager pGraphics)
    {
        _graphics = pGraphics;
    }
}
