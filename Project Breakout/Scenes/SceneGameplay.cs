using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ProjectBreakout
{
    internal class SceneGameplay : Scene
    {
        Paddle Paddle { get; set; }
        Ball Ball { get; set; }
        List<Brick> BrickList { get; set; }

        public SceneGameplay() : base()
        {
            Paddle = new Paddle("Paddle_violet_large");
            Ball = new Ball("Ball_violet");
            BrickList = new List<Brick>();
        }

        public override void Load()
        {
            Paddle.Load();
            Ball.Load();

            string levelJson = File.ReadAllText("level1.json");
            Level CurrentLevel = JsonSerializer.Deserialize<Level>(levelJson);

            string textureBrick;

            string[] textureAllBrick = new string[3];
            textureAllBrick[0] = "brick_green_1";
            textureAllBrick[1] = "brick_orange_1";
            textureAllBrick[2] = "brick_violet_1";

            for (int l = 0; l < 10; l++)
            {
                for (int c = 0; c < 18; c++)
                {
                    int typeBrick = CurrentLevel.Map[l][c];
                    if (typeBrick != 0)
                    {
                        textureBrick = textureAllBrick[typeBrick - 1];
                        Brick newBrick = new Brick(textureBrick);
                        newBrick.SetPosition(new Vector2(c * newBrick.Width, l * newBrick.Height));
                        BrickList.Add(newBrick);
                    }
                }
            }

            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Paddle.Draw(gameTime);
            Ball.Draw(gameTime);

            foreach (Brick brick in BrickList)
            {
                brick.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
