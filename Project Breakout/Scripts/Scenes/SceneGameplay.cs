using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Color = Microsoft.Xna.Framework.Color;

namespace ProjectBreakout;

internal class SceneGameplay : Scene
{
    public Paddle Paddle { get; private set; }
    public Ball Ball { get; private set; }

    public ListBricks ListBricks { get; private set; }

    public List<Ball> ListBall { get; private set; }
    public List<Bonus> ListBonus { get; private set; }
    public List<Bonus> ListActiveBonus { get; private set; }
    public List<Enemy> ListEnemy { get; private set; }

    public bool StickyBall { get; private set; }
    public bool CollisionBrick { get; private set; }
    public bool CollisionPaddle { get; private set; }

    public int CurrentScore { get; private set; }

    KeyboardState newKeyboardState;
    KeyboardState oldKeyboardState;

    public string[] BonusType { get; private set; }

    float timerEnemy;
    float timerBonus;
    bool activeBonus;

    public SoundEffect HitSound { get; private set; }
    public Song Gameplay { get; private set; }

    Random random;

    public SceneGameplay() : base()
    {
        Paddle = new("Paddle", "Blue", "Large");
        Ball = new("Ball", Paddle.Type);

        ListBricks = new();
        ListBonus = new();
        ListActiveBonus = new();
        ListBall = new();
        ListEnemy = new();

        StickyBall = true;
        CollisionBrick = false;
        CollisionPaddle = true;

        CurrentScore = 0;
        oldKeyboardState = Keyboard.GetState();
        random = new();

        timerBonus = 15;
        timerEnemy = random.Next(5, 11);

        HitSound = _assets.GetSoundEffect("HitSound");
        Gameplay = _assets.GetSong("Gameplay");

        MediaPlayer.Play(Gameplay);
        MediaPlayer.IsRepeating = true;
    }

    public override void Load()
    {
        base.Load();

        // Load Level
        string fileName = "../../../Levels/level_1.json";
        string levelJsonString = File.ReadAllText(fileName);
        Level currentLevel = JsonSerializer.Deserialize<Level>(levelJsonString);

        // Load Sprites
        Paddle.Load();

        Ball.SetPosition(
            Paddle.Position.X + Paddle.Width / 2 - Ball.Width / 2,
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
    }

    public override void Unload()
    {
        MediaPlayer.Stop();
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
            if (!HitSound.Play())
            {
                HitSound.Play();
            }
            Ball.ChangeDirectionX();
            CollisionPaddle = true;
            Debug.WriteLine("Paddle Collision");
            Debug.WriteLine("The ball is " + Ball.Type.ToString());
        }
        else if (Paddle.BoundingBox.Intersects(Ball.NextPositionY()))
        {
            if (!HitSound.Play())
            {
                HitSound.Play();
            }
            Ball.ChangeDirectionY();
            CollisionPaddle = true;
            Debug.WriteLine("Paddle Collision");
            Debug.WriteLine("The ball is " + Ball.Type.ToString());
        }
        else
        {
            CollisionPaddle = false;
        }

        if (StickyBall || CollisionPaddle && Ball.BT != Ball.BallType.Big)
        {
            if (Paddle.PT == Paddle.PaddleType.Blue)
            {
                Ball.ChangeType(Ball.BallType.Blue);
            }
            else if (Paddle.PT == Paddle.PaddleType.Red)
            {
                Ball.ChangeType(Ball.BallType.Red);
            }
            else if (Paddle.PT == Paddle.PaddleType.Yellow)
            {
                Ball.ChangeType(Ball.BallType.Yellow);
            }
        }

        // BRICKS
        for (int i = ListBricks.Bricks.Count - 1; i >= 0; i--)
        {
            Brick brick = ListBricks.Bricks[i];
            brick.Update(gameTime);

            foreach (Ball ball in ListBall)
            {
                if (brick.BoundingBox.Intersects(ball.NextPositionX()))
                {
                    if (!HitSound.Play())
                    {
                        HitSound.Play();
                    }
                    ball.ChangeDirectionX();
                    CollisionBrick = true;
                    Debug.WriteLine("Brick Collision");
                }
                else if (brick.BoundingBox.Intersects(ball.NextPositionY()))
                {
                    if (!HitSound.Play())
                    {
                        HitSound.Play();
                    }
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
            }

            if (brick.BoundingBox.Intersects(Ball.NextPositionX()))
            {
                if (!HitSound.Play())
                {
                    HitSound.Play();
                }
                Ball.ChangeDirectionX();
                CollisionBrick = true;
                Debug.WriteLine("Brick Collision");
            }
            else if (brick.BoundingBox.Intersects(Ball.NextPositionY()))
            {
                if (!HitSound.Play())
                {
                    HitSound.Play();
                }
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
                int proba = random.Next(1, 7);
                Debug.WriteLine("Proba = " + proba);

                if (proba >= 4 && !activeBonus)
                {
                    Bonus randomBonus = new(BonusType[random.Next(0, BonusType.Length)]);

                    randomBonus.SetPosition(
                        brick.Position.X + brick.Width / 2 - randomBonus.Width / 2,
                        brick.Position.Y);

                    ListBonus.Add(randomBonus);
                }
            }
        }

        if (Ball.Position.Y - Ball.Height * 2 >= _screenSize.height && ListBall.Count == 0)
        {
            Paddle.Life--;
            StickyBall = true;

            if (Paddle.Life == 0)
            {
                _gameState.ChangeScene(GameState.SceneType.Gameover);
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
                else if (bonus.NameImage == "multiball")
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        Ball newBall = new("Ball", Ball.Type);
                        newBall.SetPosition(Ball.Position.X, Ball.Position.Y);

                        foreach (Ball b in ListBall)
                        {
                            newBall = new("Ball", b.Type);
                            newBall.SetPosition(b.Position.X, b.Position.Y);
                        }

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

                        newBall.Speed = new Vector2(speed_x, speed_y);
                        ListBall.Add(newBall);
                    }
                    activeBonus = false;
                    ListActiveBonus.Remove(bonus);
                }
                else if (bonus.NameImage == "slow_ball")
                {
                    timerBonus -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Ball.Speed = new Vector2(Ball.Speed.X / 1.5f, Ball.Speed.Y / 1.5f);
                    activeBonus = false;
                    ListActiveBonus.Remove(bonus);
                }
                else if (bonus.NameImage == "small_bar")
                {
                    timerBonus -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Paddle.ChangeState(Paddle.PaddleState.Small);

                    if (timerBonus <= 0)
                    {
                        Paddle.ChangeState(Paddle.PaddleState.Large);
                        timerBonus = 15;
                        activeBonus = false;
                        ListActiveBonus.Remove(bonus);
                    }
                }
            }
        }

        // MULTIBALL
        for (int j = ListBall.Count - 1; j >= 0; j--)
        {
            Ball newBall = ListBall[j];
            newBall.Move();
            newBall.Update(gameTime);

            if (Paddle.BoundingBox.Intersects(newBall.NextPositionX()))
            {
                newBall.ChangeDirectionX();
                CollisionPaddle = true;
                Debug.WriteLine("Paddle Collision");
                Debug.WriteLine("The ball is " + newBall.Type.ToString());
            }
            else if (Paddle.BoundingBox.Intersects(newBall.NextPositionY()))
            {
                newBall.ChangeDirectionY();
                CollisionPaddle = true;
                Debug.WriteLine("Paddle Collision");
                Debug.WriteLine("The ball is " + newBall.Type.ToString());
            }
            else
            {
                CollisionPaddle = false;
            }

            if (CollisionPaddle && newBall.BT != Ball.BallType.Big)
            {
                if (Paddle.PT == Paddle.PaddleType.Blue)
                {
                    Ball.ChangeType(Ball.BallType.Blue);
                }
                else if (Paddle.PT == Paddle.PaddleType.Red)
                {
                    Ball.ChangeType(Ball.BallType.Red);
                }
                else if (Paddle.PT == Paddle.PaddleType.Yellow)
                {
                    Ball.ChangeType(Ball.BallType.Yellow);
                }
            }

            if (newBall.Position.Y - newBall.Height * 2 >= _screenSize.height)
            {
                ListBall.Remove(newBall);
            }
        }

        // ENEMY

        timerEnemy -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

        Enemy[] newEnemy = new Enemy[2];
        newEnemy[0] = new EnemyBlue("Enemy_1");
        newEnemy[1] = new EnemyYellow("Enemy_2");

        if (timerEnemy <= 0 && ListEnemy.Count < 3)
        {
            Enemy enemy = newEnemy[random.Next(0, newEnemy.Length)];
            enemy.SetPosition(random.Next(0, _screenSize.width - enemy.Width), 0);
            ListEnemy.Add(enemy);

            timerEnemy = random.Next(5, 11);
        }

        for (int i = ListEnemy.Count - 1; i >= 0; i--)
        {
            Enemy enemy = ListEnemy[i];
            enemy.Update(gameTime);
            Debug.WriteLine("State Machine = " + enemy.SM);

            if (enemy.Position.Y - enemy.Height * 2 >= _screenSize.height)
            {
                ListEnemy.Remove(enemy);
            }
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        Paddle.Draw(gameTime);
        Ball.Draw(gameTime);

        ListBricks.Draw();

        foreach (Ball b in ListBall)
        {
            _spriteBatch.Draw(b.SpriteTexture, b.Position, Color.White);
        }

        foreach (Bonus b in ListBonus)
        {
            _spriteBatch.Draw(b.SpriteTexture, b.Position, Color.White);
        }

        foreach (Enemy e in ListEnemy)
        {
            _spriteBatch.Draw(e.SpriteTexture, e.Position, Color.White);
        }

        _spriteBatch.DrawString(
            _assets.GetFont("SubTitle"),
            CurrentScore.ToString(),
            new Vector2(10, _screenSize.height - 32),
            Color.White);

        _spriteBatch.DrawString(
                _assets.GetFont("Text"),
                string.Format(
                "Ball list count = {0}" + "\n" +
                "Timer Bonus = {1}" + "\n" +
                "Bonus active = {2}" + "\n" +
                "Timer Enemy = {3}",
                ListBall.Count, Math.Ceiling(timerBonus), activeBonus.ToString(), Math.Ceiling(timerEnemy)),
                new Vector2(_screenSize.width - 150, _screenSize.height - 70),
                Color.White);
    }
}
