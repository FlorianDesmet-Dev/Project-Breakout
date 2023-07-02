using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace ProjectBreakout;

internal class SceneGameplay : Scene
{
    public Paddle Paddle { get; private set; }
    public Ball Ball { get; private set; }

    public ListBricks ListBricks { get; private set; }
    // public ListBalls ListBalls { get; private set; }

    public List<Ball> ListBall { get; private set; } 
    public List<Bonus> ListBonus { get; private set; }
    public List<Bonus> ListActiveBonus { get; private set; }

    public bool StickyBall { get; private set; }
    public bool CollisionBrick { get; private set; }
    public bool CollisionPaddle { get; private set; }
    
    public int CurrentScore { get; private set; }

    KeyboardState newKeyboardState;
    KeyboardState oldKeyboardState;

    public string[] BonusType { get; private set; }
    float timerBonus;
    bool activeBonus;

    public SceneGameplay() : base()
    {
        Paddle = new("Paddle", "Blue", "Large");
        Ball = new("Ball", Paddle.Type);

        ListBricks = new();
        ListBonus = new();
        ListActiveBonus = new();
        ListBall = new();

        StickyBall = true;
        CollisionBrick = false;
        CollisionPaddle = true;

        CurrentScore = 0;
        oldKeyboardState = Keyboard.GetState();

        timerBonus = 15;
    }

    public override void Load()
    {
        // Load Level
        string fileName = "../../../Levels/level_1.json";
        string levelJsonString = File.ReadAllText(fileName);
        Level currentLevel = JsonSerializer.Deserialize<Level>(levelJsonString);

        // Load Sprites
        Paddle.Load();

        Ball.SetPosition(
            Paddle.Position.X + (Paddle.Width / 2) - (Ball.Width / 2),
            Paddle.Position.Y - Ball.Height);
        Ball.Load();

        ListBricks.Load(currentLevel.Lines, currentLevel.Columns, currentLevel.Map);

        BonusType = new string[7];
        BonusType[0] = "big_ball";
        BonusType[1] = "big_bar";
        BonusType[2] = "fast_ball";
        BonusType[3] = "inverted_commands";
        BonusType[4] = "multiball";
        BonusType[5] = "slow_ball";
        BonusType[6] = "small_bar";
        
        StickyBall = true;

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
            StickyBall = false;
            CollisionPaddle = false;
        }
        
        // PADDLE
        Paddle.Controller();
        Paddle.Update(gameTime);

        newKeyboardState = Keyboard.GetState();

        if (newKeyboardState.IsKeyDown(Keys.Space) &&
            oldKeyboardState != newKeyboardState)
        {

        }

        oldKeyboardState = newKeyboardState;

        // BALL
        Ball.Move();
        Ball.Update(gameTime);

        if (StickyBall)
        {
            Ball.StickyBall(Paddle);
        }

        if (Paddle.BoundingBox.Intersects(Ball.NextPositionX()))
        {
            Ball.ChangeDirectionX();
            CollisionPaddle = true;
            Debug.WriteLine("Paddle Collision");
            Debug.WriteLine("The ball is " + Ball.Type.ToString());
        }
        else if (Paddle.BoundingBox.Intersects(Ball.NextPositionY()))
        {
            Ball.ChangeDirectionY();
            CollisionPaddle = true;
            Debug.WriteLine("Paddle Collision");
            Debug.WriteLine("The ball is " + Ball.Type.ToString());
        }
        else
        {
            CollisionPaddle = false;
        }

        if (StickyBall || CollisionPaddle && Ball.Type != "Big")
        {
            if (Paddle.Type == "Blue")
            {
                Ball.ChangeType(Ball.BallType.Blue);
            }
            else if (Paddle.Type == "Red")
            {
                Ball.ChangeType(Ball.BallType.Red);
            }
            else if (Paddle.Type == "Yellow")
            {
                Ball.ChangeType(Ball.BallType.Yellow);
            }
        }

        // BRICKS
        for (int i = ListBricks.Bricks.Count - 1; i >= 0; i--)
        {
            Brick brick = ListBricks.Bricks[i];
            brick.Update(gameTime);

            /* foreach (Ball ball in ListBalls.Balls)
            {
                if (brick.BoundingBox.Intersects(ball.NextPositionX()))
                {
                    ball.ChangeDirectionX();
                    CollisionBrick = true;
                    Debug.WriteLine("Brick Collision");
                }
                else if (brick.BoundingBox.Intersects(ball.NextPositionY()))
                {
                    ball.ChangeDirectionY();
                    CollisionBrick = true;
                    Debug.WriteLine("Brick Collision");
                }
                else
                {
                    CollisionBrick = false;
                }

                if (CollisionBrick && brick.Type == ball.Type)
                {
                    brick.Life--;
                    CurrentScore = ScoreManager.IncrementScore(10);
                }
            } */
      
            if (brick.BoundingBox.Intersects(Ball.NextPositionX()))
            {
                Ball.ChangeDirectionX();
                CollisionBrick = true;
                Debug.WriteLine("Brick Collision");
            }
            else if (brick.BoundingBox.Intersects(Ball.NextPositionY()))
            {
                Ball.ChangeDirectionY();
                CollisionBrick = true;
                Debug.WriteLine("Brick Collision");
            }
            else
            {
                CollisionBrick = false;
            }

            if (CollisionBrick && (brick.Type == Ball.Type || Ball.Type == "Big"))
            {
                brick.Life--;
                CurrentScore = ScoreManager.IncrementScore(10);                    
            }

            if (brick.Life <= 0)
            {
                ListBricks.Bricks.Remove(brick);
                Debug.WriteLine("Remove brick");

                // CREATE BONUS
                Random random = new();
                int proba = 1;
                Debug.WriteLine("Proba = " + proba);

                if (proba == 1 && !activeBonus)
                {
                    Bonus randomBonus = new(BonusType[1]);

                    randomBonus.SetPosition(
                        brick.Position.X + (brick.Width / 2) - (randomBonus.Width / 2),
                        brick.Position.Y);

                    ListBonus.Add(randomBonus);
                }
            }
        }        

        /* oldBallType = Ball.Type;

        // MULTIBALL
        for (int j = 0; j <= 9; j++)
        {
            Ball newBall = new("Ball", Ball.Type);
            newBall.SetPosition(Ball.Position.X, Ball.Position.Y);

            Random random = new();
            int speed_x;
            int speed_y;
            do
            {
                speed_x = random.Next(-4, 4);
            } while (speed_x == 0);

            do
            {
                speed_y = random.Next(-4, 4);
            } while (speed_y == 0);

            // newBall.Velocity = new Vector2(speed_x, speed_y);
            // ListBall.Add(newBall);
        } */

        if (Ball.Position.Y - (Ball.Height * 2) >= ScreenSize.height && ListBall.Count == 0)
        {
            Paddle.Life--;
            StickyBall = true;

            if (Paddle.Life == 0)
            {
                GameState.ChangeScene(GameState.SceneType.Gameover);
            }
        }

        // BONUS
        for (int j = ListBonus.Count - 1; j >= 0; j--)
        {
            Bonus bonus = ListBonus[j];
            bonus.Move();
            bonus.Update(gameTime);

            if (bonus.BoundingBox.Intersects(Paddle.BoundingBox))
            {
                activeBonus = true;
                ListActiveBonus.Add(bonus);
                ListBonus.Remove(bonus);
            }
        }

        if (activeBonus)
        {
            for (int i = ListActiveBonus.Count - 1; i >= 0; i--)
            {
                Bonus bonus = ListActiveBonus[i];

                if (bonus.NameImage == "big_ball")
                {
                    timerBonus -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Ball.ChangeType(Ball.BallType.Big);

                    if (timerBonus <= 0)
                    {
                        Ball.ChangeType(Ball.BallType.Blue);
                        timerBonus = 15;
                        activeBonus = false;
                        ListActiveBonus.Remove(bonus);
                    }
                }
                else if (bonus.NameImage == "big_bar")
                {
                    timerBonus -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Paddle.ChangeState(Paddle.PaddleState.XXLarge);
                    
                    if (timerBonus <= 0)
                    {
                        Paddle.ChangeState(Paddle.PaddleState.Large);
                        timerBonus = 15;
                        activeBonus = false;
                        ListActiveBonus.Remove(bonus);
                    }
                }
                else if (bonus.NameImage == "fast_ball")
                {
                    timerBonus -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Ball.Speed = new Vector2(Ball.Speed.X * 1.5f, Ball.Speed.Y * 1.5f);
                    activeBonus = false;
                    ListActiveBonus.Remove(bonus);
                }
            }
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Paddle.Draw(gameTime);
        Ball.Draw(gameTime);

        ListBricks.Draw();

        foreach (Ball b in ListBall)
        {
            Batch.Draw(b.SpriteTexture, b.Position, Color.White);
        }

        foreach (Bonus b in ListBonus)
        {
            Batch.Draw(b.SpriteTexture, b.Position, Color.White);
        }

        Batch.DrawString(
            Asset.GetFont("SubTitle"), 
            "Score : " + CurrentScore,
            new Vector2(10, 10),
            Color.White);
        
        Batch.DrawString(
                Asset.GetFont("Text"),
                String.Format(
                    "Ball list count = {0} Timer Bonus = {1} Bonus active = {2}", 
                    ListBall.Count, Math.Ceiling(timerBonus), activeBonus.ToString()),
                new Vector2(10, ScreenSize.height - 50),
                Color.White);

        base.Draw(gameTime);
    }
}
