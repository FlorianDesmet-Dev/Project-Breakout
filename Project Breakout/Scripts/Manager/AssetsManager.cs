using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ProjectBreakout;
using System;

internal class AssetsManager : IGetAssets
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

    public SoundEffect GetSoundEffect(String pNameSound)
    {
        return content.Load<SoundEffect>(pNameSound);
    }

    public Song GetSong(String pNameSong)
    {
        return content.Load<Song>(pNameSong);
    }
}
