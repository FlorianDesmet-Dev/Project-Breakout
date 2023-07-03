using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ProjectBreakout;

internal interface IGetAssets
{
    Texture2D GetTexture(string pNameImage);
    SpriteFont GetFont(string pNameFont);
    SoundEffect GetSoundEffect(string pNameSoundEffect);
    Song GetSong(string pNameSong);
}
