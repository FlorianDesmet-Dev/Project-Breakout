using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ProjectBreakout
{
    internal class SceneGameplay : Scene
    {
        public Paddle Paddle { get; set; }
        public Ball Ball { get; set; }

        Brick NewBrick { get; set; }
        public List<Brick> BrickList { get; set; }

        Bonus NewBonus { get; set; }
        public List<Bonus> BonusList { get; set; }

        public bool BallStick { get; set; }
        public int CurrentScore { get; set; }

        public string oldTextureBonus { get; set; }

        public SceneGameplay() : base()
        {
            Paddle = new Paddle("green", "large");
            Ball = new Ball(Paddle.Type, "");
            BrickList = new List<Brick>();
            BonusList = new List<Bonus>();

            CurrentScore = 0;
        }

        public override void Load()
        {
            // Loading Level
            string fileName = "../../../Levels/level_1.json";
            string levelJsonString = File.ReadAllText(fileName);
            Level currentLevel = JsonSerializer.Deserialize<Level>(levelJsonString);

            // Loading Sprite
            Paddle.Load();

            Ball.SetPosition(
                (ScreenSize.width / 2) - (Paddle.Width / 2),
                (ScreenSize.height) - (Paddle.Height * 2));
            Ball.Load();

            BallStick = true;

            string[] allType = new string[3];
            allType[0] = "green";
            allType[1] = "orange";
            allType[2] = "violet";

            for (int l = 0; l < 10; l++)
            {
                for (int c = 0; c < 18; c++)
                {
                    int brickType = currentLevel.Map[l][c];
                    if (brickType != 0)
                    {
                        string type;
                        type = allType[brickType - 1];
                        NewBrick = new Brick(type, "intact");
                        NewBrick.SetPosition((c * NewBrick.Width), (l * NewBrick.Height));
                        BrickList.Add(NewBrick);
                    }
                }
            }

            base.Load();
        }

        public override void Unload()
        {
            ScoreManager.SaveScore();
            base.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                BallStick = false;
            }

            if (!BallStick)
            {
                Paddle.Controller();
            }

            Paddle.Update(gameTime);

            // Bricks
            for (int i = BrickList.Count - 1; i >= 0; i--)
            {
                bool brickCollision;
                Brick brick = BrickList[i];
                brick.Update(gameTime);

                if (brick.BoundingBox.Intersects(Ball.NextPositionX()))
                {
                    Ball.ChangeDirectionX();
                    brickCollision = true;
                }
                else if (brick.BoundingBox.Intersects(Ball.NextPositionY()))
                {
                    Ball.ChangeDirectionY();
                    brickCollision = true;
                }
                else
                {
                    brickCollision = false;
                }

                if (brickCollision)
                {
                    if (brick.Type == Ball.Type)
                    {
                        brick.Strength--;
                        CurrentScore = ScoreManager.IncrementScore(10);
                    }

                    switch (brick.Strength)
                    {
                        case 1:
                            brick.ChangeState(Brick.BrickState.Break);
                            break;
                        case 2:
                            brick.ChangeState(Brick.BrickState.Crack);
                            break;
                        default:
                            break;
                    }                   

                    if (brick.Strength <= 0)
                    {
                        BrickList.Remove(brick);
                        
                        // Create Bonus
                        string[] listTextureBonus = new string[2];
                        listTextureBonus[0] = "Bonus_big_bar";
                        listTextureBonus[1] = "Bonus_multiball";

                        Random random = new Random();

                        string newTextureBonus = listTextureBonus[random.Next(0, listTextureBonus.Length)];

                        if (BonusList.Count == 0 &&
                            oldTextureBonus != newTextureBonus)
                        {                            
                            NewBonus = new Bonus(newTextureBonus);
                            NewBonus.SetPosition(brick.Position.X, brick.Position.Y);
                            BonusList.Add(NewBonus);
                        }
                        oldTextureBonus = newTextureBonus;
                    }                    
                }
            }

            // Update and Remove Bonus
            for (int i = BonusList.Count; i > 0; i--)
            {               
                BonusList[i - 1].Update(gameTime);
                
                if (BonusList[i - 1].Position.Y >= (ScreenSize.height + BonusList[i - 1].Height) ||
                    BonusList[i - 1].BoundingBox.Intersects(Paddle.BoundingBox))
                {
                    BonusList.Remove(BonusList[i - 1]);
                }
            }            

            // Ball
            Ball.Update(gameTime);
            bool ballCollision;

            if (BallStick)
            {
                Ball.SetPosition(
                    Paddle.Position.X + (Paddle.Width / 2) - (Ball.Width / 2),
                    Paddle.Position.Y - Paddle.Height);
                Ball.Speed = new Vector2(2, -2);
            }

            if (Paddle.BoundingBox.Intersects(Ball.NextPositionX()))
            {
                Ball.ChangeDirectionX();
                ballCollision = true;

            }
            else if (Paddle.BoundingBox.Intersects(Ball.NextPositionY()))
            {
                Ball.ChangeDirectionY();
                ballCollision = true;
            }
            else
            {
                ballCollision = false;
            }

            if (ballCollision)
            {
                switch (Paddle.Type)
                {
                    case "green":
                        Ball.ChangeType(Ball.BallType.Green);
                        break;
                    case "orange":
                        Ball.ChangeType(Ball.BallType.Orange);
                        break;
                    case "violet":
                        Ball.ChangeType(Ball.BallType.Violet);
                        break;
                    default:
                        break;
                }
            }

            if (Ball.Position.Y - (Ball.Height * 2) >= ScreenSize.height)
            {
                // Life --
                BallStick = true;
                // GameState.ChangeScene(GameState.SceneType.Gameover);
            }

            /* if (Life <= 0)
            {
                Game over !!!   
            }   */

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

            foreach (Bonus bonus in BonusList)
            {
                bonus.Draw(gameTime);
            }

            Batch.DrawString(Asset.GetFont("SubTitle"), "Score : " + CurrentScore, new Vector2(10, 10), Color.White);
            
            Batch.DrawString(
                    Asset.GetFont("SubTitle"),
                    "Bonus list count = " + BonusList.Count,
                    new Vector2(200, 10),
                    Color.White);

            base.Draw(gameTime);
        }
    }
}
