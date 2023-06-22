using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

public interface IGetAsset
{
    Texture2D GetTexture(string pNameImage);
    SpriteFont GetFont(string pNameFont);
}

internal class AssetsManager : IGetAsset
{
    private ContentManager content;

    public AssetsManager(ContentManager pContent)
    {
        content = pContent;
    }

    public SpriteFont GetFont(string pNameFont)
    {
        return content.Load<SpriteFont>(pNameFont);
    }

    public Texture2D GetTexture(String pNameImage)
    {
        return content.Load<Texture2D>(pNameImage);
    }
}
