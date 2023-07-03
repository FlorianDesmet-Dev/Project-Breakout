using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ProjectBreakout;

internal class ListBricks
{
    public SpriteBatch Batch { get; private set; }

    public List<Brick> Bricks { get; private set; }
    public ListBricks()
    {
        Batch = ServiceLocator.GetService<SpriteBatch>();
        Bricks = new();
    }

    public void Load(int pLines, int pColumns, int[][] pMap)
    {
        string[] allType = new string[3];
        allType[0] = "Blue";
        allType[1] = "Red";
        allType[2] = "Yellow";

        for (int l = 0; l < pLines; l++)
        {
            for (int c = 0; c < pColumns; c++)
            {
                int brickType = pMap[l][c];
                if (brickType != 0)
                {
                    string type;
                    type = allType[brickType - 1];
                    Brick NewBrick = new("Brick", type, "Full");
                    NewBrick.SetPosition(c * NewBrick.Width, l * NewBrick.Height);
                    Bricks.Add(NewBrick);
                }
            }
        }
    }

    public void Draw()
    {
        foreach (Brick b in Bricks)
        {
            Batch.Draw(b.SpriteTexture, b.Position, Color.White);
        }
    }
}
