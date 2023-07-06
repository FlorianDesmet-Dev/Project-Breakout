using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using Color = Microsoft.Xna.Framework.Color;

namespace ProjectBreakout;

internal class SceneGameplay : Scene
{
    public LevelManager CurrentLevel { get; private set; }

    public Paddle Paddle { get; private set; }
    public Ball Ball { get; private set; }

    public List<Ball> ListBall { get; private set; }
    public List<Bonus> ListBonus { get; private set; }
    public List<Bonus> ListActiveBonus { get; private set; }
    public List<EnemyBlue> ListEnemyBlue { get; private set; }
    public List<EnemyYellow> ListEnemyYellow { get; private set; }

    private Vector2 OldSpeedBall { get; set; }

    public bool StickyBall { get; private set; }
    public bool CollisionBrick { get; private set; }
    public bool CollisionPaddle { get; private set; }

    public int CurrentScore { get; private set; }
    public string[] BonusType { get; private set; }

    public Song SongGameplay { get; private set; }    
    
    float timerEnemy;
    float timerBonus;

    bool invertedCommands;
    bool fastBall;
    bool slowBall;
    bool multiBall;

    KeyboardState newKeyboardState;
    KeyboardState oldKeyboardState;

    Random random;

    public SceneGameplay() : base()
    {
        CurrentLevel = new();

        Paddle = new("Paddle", "Blue", "Large");
        Ball = new("Ball", Paddle.Color);

        ListBonus = new();
        ListActiveBonus = new();
        ListBall = new();
        ListEnemyBlue = new();
        ListEnemyYellow = new();

        StickyBall = true;
        CollisionBrick = false;
        CollisionPaddle = true;

        CurrentScore = 0;
        random = new();

        timerBonus = 0;
        timerEnemy = random.Next(10, 21);

        SongGameplay = _assets.GetSong("Gameplay");

        MediaPlayer.Play(SongGameplay);
        MediaPlayer.IsRepeating = true;

        oldKeyboardState = Keyboard.GetState();
    }

    public override void Load()
    {
        base.Load();

        // Load Level
        CurrentLevel.LoadLevel(1);
        Debug.WriteLine("Author = " + CurrentLevel.Level.Author);
        Debug.WriteLine("Lines = " + CurrentLevel.Level.Lines);
        Debug.WriteLine("Columns = " + CurrentLevel.Level.Columns);

        // Load Sprites
        Paddle.Load();

        Ball.SetPosition(
            Paddle.Position.X + Paddle.Width / 2 - Ball.Width / 2,
            Paddle.Position.Y - Ball.Height / 2);
        Ball.Load();

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

    public void CreateBonus(Brick pBrick)
    {
        int probaBonus = random.Next(0, 2);
        Debug.WriteLine("Proba = " + probaBonus);

        if (probaBonus != 0 && timerBonus <= 0 && !multiBall && !fastBall && !slowBall)
        {
            Bonus randomBonus = new(BonusType[random.Next(0, BonusType.Length)]);
            randomBonus.SetPosition(
            pBrick.Position.X + pBrick.Width / 2 - randomBonus.Width / 2,
                pBrick.Position.Y);

            ListBonus.Add(randomBonus);
        }
    }

    public override void Update(GameTime gameTime)
    {
        foreach (Background background in Backgrounds)
        {
            background.Update(gameTime);
        }

        newKeyboardState = Keyboard.GetState();

        if (newKeyboardState.IsKeyDown(Keys.Space) &&
            oldKeyboardState != newKeyboardState)
        {
            StickyBall = false;
            CollisionPaddle = false;
        }

        oldKeyboardState = newKeyboardState;

        // PADDLE
        if (!invertedCommands)
        {
            Paddle.Commands();
        }
        else
        {
            Paddle.InvertedCommands();
        }

        Paddle.Update(gameTime);

        if (Paddle.Life == 0)
        {
            _gameState.ChangeScene(GameState.SceneType.Gameover);
        }

        // BALL
        Ball.Move();
        Ball.Update(gameTime);

        if (StickyBall)
        {
            Ball.StickyBall(Paddle);
        }

        if (Paddle.BoundingBox.Intersects(Ball.NextPositionX()) &&
            !StickyBall)
        {
            Ball.HitX();
            CollisionPaddle = true;
        }
        else if (Paddle.BoundingBox.Intersects(Ball.NextPositionY()) &&
            !StickyBall)
        {
            Ball.HitY();
            CollisionPaddle = true;
        }
        else
        {
            CollisionPaddle = false;
        }

        if (StickyBall || CollisionPaddle && Ball.BType != Ball.BallColor.Big)
        {
            if (Paddle.PColor == Paddle.PaddleColor.Blue)
            {
                Ball.ChangeType(Ball.BallColor.Blue);
            }
            else if (Paddle.PColor == Paddle.PaddleColor.Red)
            {
                Ball.ChangeType(Ball.BallColor.Red);
            }
            else if (Paddle.PColor == Paddle.PaddleColor.Yellow)
            {
                Ball.ChangeType(Ball.BallColor.Yellow);
            }
        }

        // BRICKS
        for (int i = CurrentLevel.Level.ListBricks.Bricks.Count - 1; i >= 0; i--)
        {
            Brick brick = CurrentLevel.Level.ListBricks.Bricks[i];
            brick.Update(gameTime);

            foreach (Ball ball in ListBall)
            {
                if (brick.BoundingBox.Intersects(ball.NextPositionX()))
                {
                    ball.HitX();
                    CollisionBrick = true;
                }
                else if (brick.BoundingBox.Intersects(ball.NextPositionY()))
                {
                    ball.HitY();
                    CollisionBrick = true;
                }
                else
                {
                    CollisionBrick = false;
                }

                if (CollisionBrick && brick.Color == ball.Color)
                {
                    brick.Life--;
                    CurrentScore = ScoreManager.IncrementScore(10);
                }
            }

            if (brick.BoundingBox.Intersects(Ball.NextPositionX()))
            {
                Ball.HitX();
                CollisionBrick = true;
            }
            else if (brick.BoundingBox.Intersects(Ball.NextPositionY()))
            {
                Ball.HitY();
                CollisionBrick = true;
            }
            else
            {
                CollisionBrick = false;
            }

            if (CollisionBrick && (brick.Color == Ball.Color || Ball.Color == "Big"))
            {
                brick.Life--;
                
                if (brick.Life >= 1)
                {
                    CurrentScore = ScoreManager.IncrementScore(10);
                }
            }

            foreach (Enemy enemyYellow in ListEnemyYellow)
            {
                if (brick.BoundingBox.Intersects(enemyYellow.NextPositionX()))
                {
                    enemyYellow.ChangeDirectionX();

                    if (brick.Life == 1)
                    {
                        brick.Life = 2;
                    }
                    else if (brick.Life == 2)
                    {
                        brick.Life = 3;
                    }
                }
                else if (brick.BoundingBox.Intersects(enemyYellow.NextPositionY()))
                {
                    enemyYellow.ChangeDirectionY();

                    if (brick.Life == 1)
                    {
                        brick.Life = 2;
                    }
                    else if (brick.Life == 2)
                    {
                        brick.Life = 3;
                    }
                }
            }

            if (brick.Life <= 0)
            {
                CurrentScore = ScoreManager.IncrementScore(100);
                CurrentLevel.Level.ListBricks.Bricks.Remove(brick);
                Debug.WriteLine("Remove brick");

                // CREATE BONUS
                CreateBonus(brick);
            }
        }

        if (Ball.Position.Y - Ball.Height * 2 >= _screenSize.height && ListBall.Count == 0)
        {
            Paddle.Life--;
            StickyBall = true;
        }

        // BONUS
        for (int j = ListBonus.Count - 1; j >= 0; j--)
        {
            Bonus bonus = ListBonus[j];
            bonus.Move();
            bonus.Update(gameTime);

            if (bonus.BoundingBox.Intersects(Paddle.BoundingBox))
            {
                ListActiveBonus.Add(bonus);
                ListBonus.Remove(bonus);
            }
        }

        for (int i = ListActiveBonus.Count - 1; i >= 0; i--)
        {
            Bonus bonus = ListActiveBonus[i];

            if (bonus.NameImage == "big_ball")
            {
                timerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Ball.ChangeType(Ball.BallColor.Big);

                if (timerBonus >= 10 || StickyBall)
                {
                    Ball.ChangeType(Ball.BallColor.Blue);
                    timerBonus = 0;
                    ListActiveBonus.Remove(bonus);
                }
            }
            else if (bonus.NameImage == "big_bar")
            {
                timerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Paddle.ChangeState(Paddle.PaddleLenght.XXLarge);

                if (timerBonus >= 10)
                {
                    Paddle.ChangeState(Paddle.PaddleLenght.Large);
                    timerBonus = 0;
                    ListActiveBonus.Remove(bonus);
                }
            }
            else if (bonus.NameImage == "fast_ball")
            {
                OldSpeedBall = new Vector2(Math.Abs(Ball.Speed.X), Math.Abs(Ball.Speed.Y));
                Debug.WriteLine("Old Speed ball = " + OldSpeedBall);
                Ball.FastBall();
                fastBall = true;
                ListActiveBonus.Remove(bonus);
            }
            else if (bonus.NameImage == "inverted_commands")
            {
                invertedCommands = true;

                timerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timerBonus >= 10)
                {
                    invertedCommands = false;
                    timerBonus = 0;
                    ListActiveBonus.Remove(bonus);
                }
            }
            else if (bonus.NameImage == "multiball")
            {
                multiBall = true;

                for (int j = 0; j <= 9; j++)
                {
                    Ball newBall = new("Ball", Ball.Color);
                    newBall.SetPosition(Ball.Position.X, Ball.Position.Y);

                    foreach (Ball b in ListBall)
                    {
                        newBall = new("Ball", b.Color);
                        newBall.SetPosition(b.Position.X, b.Position.Y);
                    }

                    int speed_x;
                    int speed_y;
                    do
                    {
                        speed_x = random.Next(-4, 4);
                    } while (speed_x < 2 && speed_x > -2);

                    do
                    {
                        speed_y = random.Next(-4, 4);
                    } while (speed_y < 2 && speed_y > -2);

                    newBall.Speed = new Vector2(speed_x, speed_y);
                    ListBall.Add(newBall);
                }
                ListActiveBonus.Remove(bonus);
            }
            else if (bonus.NameImage == "slow_ball")
            {
                OldSpeedBall = new Vector2(Math.Abs(Ball.Speed.X), Math.Abs(Ball.Speed.Y));
                Debug.WriteLine("Old Speed ball = " + OldSpeedBall);
                Ball.SlowBall();
                slowBall = true;
                ListActiveBonus.Remove(bonus);
            }
            else if (bonus.NameImage == "small_bar")
            {
                timerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Paddle.ChangeState(Paddle.PaddleLenght.Small);

                if (timerBonus >= 10)
                {
                    Paddle.ChangeState(Paddle.PaddleLenght.Large);
                    timerBonus = 0;
                    ListActiveBonus.Remove(bonus);
                }
            }
        }

        if (fastBall)
        {
            timerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timerBonus >= 10)
            {
                if (Ball.Speed.X > 0 && Ball.Speed.Y > 0)
                {
                    Ball.Speed = new Vector2(OldSpeedBall.X, OldSpeedBall.Y);
                }
                else if (Ball.Speed.X < 0 && Ball.Speed.Y < 0)
                {
                    Ball.Speed = new Vector2(-OldSpeedBall.X, -OldSpeedBall.Y);
                }
                else if (Ball.Speed.X > 0 && Ball.Speed.Y < 0)
                {
                    Ball.Speed = new Vector2(OldSpeedBall.X, -OldSpeedBall.Y);
                }
                else if (Ball.Speed.X < 0 && Ball.Speed.Y > 0)
                {
                    Ball.Speed = new Vector2(-OldSpeedBall.X, OldSpeedBall.Y);
                }

                timerBonus = 0;
                fastBall = false;
            }
        }

        if (slowBall)
        {
            timerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timerBonus >= 10)
            {
                if (Ball.Speed.X > 0 && Ball.Speed.Y > 0)
                {
                    Ball.Speed = new Vector2(OldSpeedBall.X, OldSpeedBall.Y);
                }
                else if (Ball.Speed.X < 0 && Ball.Speed.Y < 0)
                {
                    Ball.Speed = new Vector2(-OldSpeedBall.X, -OldSpeedBall.Y);
                }
                else if (Ball.Speed.X > 0 && Ball.Speed.Y < 0)
                {
                    Ball.Speed = new Vector2(OldSpeedBall.X, -OldSpeedBall.Y);
                }
                else if (Ball.Speed.X < 0 && Ball.Speed.Y > 0)
                {
                    Ball.Speed = new Vector2(-OldSpeedBall.X, OldSpeedBall.Y);
                }

                timerBonus = 0;
                slowBall = false;
            }
        }

        // MULTIBALL
        if (ListBall.Count == 0)
        {
            multiBall = false;
        }

        for (int j = ListBall.Count - 1; j >= 0; j--)
        {
            Ball newBall = ListBall[j];
            newBall.Move();
            newBall.Update(gameTime);

            if (Paddle.BoundingBox.Intersects(newBall.NextPositionX()))
            {
                newBall.HitX();
                CollisionPaddle = true;
            }
            else if (Paddle.BoundingBox.Intersects(newBall.NextPositionY()))
            {
                newBall.HitY();
                CollisionPaddle = true;
            }
            else
            {
                CollisionPaddle = false;
            }

            if (CollisionPaddle && newBall.BType != Ball.BallColor.Big)
            {
                if (Paddle.PColor == Paddle.PaddleColor.Blue)
                {
                    newBall.ChangeType(Ball.BallColor.Blue);
                }
                else if (Paddle.PColor == Paddle.PaddleColor.Red)
                {
                    newBall.ChangeType(Ball.BallColor.Red);
                }
                else if (Paddle.PColor == Paddle.PaddleColor.Yellow)
                {
                    newBall.ChangeType(Ball.BallColor.Yellow);
                }
            }

            for (int i = ListEnemyBlue.Count - 1; i >= 0; i--)
            {
                EnemyBlue enemyBlue = ListEnemyBlue[i];

                if (enemyBlue.EState != Enemy.EnemyState.Appear)
                {
                    if (enemyBlue.BoundingBox.Intersects(newBall.NextPositionX()))
                    {
                        newBall.HitX();
                        CurrentScore = ScoreManager.IncrementScore(50);
                        ListEnemyBlue.Remove(enemyBlue);
                    }
                    else if (enemyBlue.BoundingBox.Intersects(newBall.NextPositionY()))
                    {
                        newBall.HitY();
                        CurrentScore = ScoreManager.IncrementScore(50);
                        ListEnemyBlue.Remove(enemyBlue);
                    }
                }
            }

            for (int i = ListEnemyYellow.Count - 1; i >= 0; i--)
            {
                EnemyYellow enemyYellow = ListEnemyYellow[i];

                if (enemyYellow.EState != Enemy.EnemyState.Appear)
                {
                    if (enemyYellow.BoundingBox.Intersects(newBall.NextPositionX()))
                    {
                        newBall.HitX();
                        CurrentScore = ScoreManager.IncrementScore(50);
                        ListEnemyYellow.Remove(enemyYellow);
                    }
                    else if (enemyYellow.BoundingBox.Intersects(newBall.NextPositionY()))
                    {
                        newBall.HitY();
                        CurrentScore = ScoreManager.IncrementScore(50);
                        ListEnemyYellow.Remove(enemyYellow);
                    }
                }
            }

            if (newBall.Position.Y - newBall.Height * 2 >= _screenSize.height)
            {
                ListBall.Remove(newBall);
            }
        }

        // ENEMY

        timerEnemy -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        int probaEnemy = random.Next(0, 2);

        if (timerEnemy <= 0 && ListEnemyBlue.Count < 5)
        {
            if (probaEnemy == 0)
            {
                EnemyBlue enemyBlue = new("Enemy_1");
                enemyBlue.SetPosition(random.Next(0, _screenSize.width - enemyBlue.Width), 0);
                ListEnemyBlue.Add(enemyBlue);
            }
            else if (probaEnemy == 1)
            {
                EnemyYellow enemyYellow = new("Enemy_2");
                enemyYellow.SetPosition(random.Next(0, _screenSize.width - enemyYellow.Width), 0);
                ListEnemyYellow.Add(enemyYellow);
            }
            
            timerEnemy = random.Next(10, 21);
        }

        for (int i = ListEnemyBlue.Count - 1; i >= 0; i--)
        {
            EnemyBlue enemyBlue = ListEnemyBlue[i];
            enemyBlue.Update(gameTime);

            if (enemyBlue.Position.Y - enemyBlue.Height * 2 >= _screenSize.height)
            {
                ListEnemyBlue.Remove(enemyBlue);
            }

            if (enemyBlue.EState != Enemy.EnemyState.Appear)
            {
                if (enemyBlue.BoundingBox.Intersects(Ball.NextPositionX()))
                {
                    Ball.HitX();
                    CurrentScore = ScoreManager.IncrementScore(50);
                    ListEnemyBlue.Remove(enemyBlue);
                }
                else if (enemyBlue.BoundingBox.Intersects(Ball.NextPositionY()))
                {
                    Ball.HitY();
                    CurrentScore = ScoreManager.IncrementScore(50);
                    ListEnemyBlue.Remove(enemyBlue);
                }
            }

            for (int j = enemyBlue.ListProjectiles.Count - 1; j >= 0; j--)
            {
                Projectile projectile = enemyBlue.ListProjectiles[j];

                if (projectile.BoundingBox.Intersects(Paddle.BoundingBox))
                {
                    enemyBlue.ListProjectiles.Remove(projectile);
                    Paddle.Life--;
                }
            }
        }

        for (int i = ListEnemyYellow.Count - 1; i >= 0; i--)
        {
            EnemyYellow enemyYellow = ListEnemyYellow[i];
            enemyYellow.Update(gameTime);

            if (enemyYellow.Position.Y - enemyYellow.Height * 2 >= _screenSize.height)
            {
                ListEnemyYellow.Remove(enemyYellow);
            }

            if (enemyYellow.EState != Enemy.EnemyState.Appear)
            {
                if (enemyYellow.BoundingBox.Intersects(Ball.NextPositionX()))
                {
                    Ball.HitX();
                    CurrentScore = ScoreManager.IncrementScore(50);
                    ListEnemyYellow.Remove(enemyYellow);
                }
                else if (enemyYellow.BoundingBox.Intersects(Ball.NextPositionY()))
                {
                    Ball.HitY();
                    CurrentScore = ScoreManager.IncrementScore(50);
                    ListEnemyYellow.Remove(enemyYellow);
                }
            }
        }

        if (CurrentLevel.Level.ListBricks.Bricks.Count == 0)
        {
            if (CurrentLevel.NumberLevel < 8)
            {
                CurrentLevel.NextLevel();
            }
            else
            {
                _gameState.ChangeScene(GameState.SceneType.Menu);
            }

            StickyBall = true;
            timerEnemy = random.Next(10, 31);
            timerBonus = 5;

            ListBonus.RemoveAll(ListBonus.Remove);
            ListEnemyBlue.RemoveAll(ListEnemyBlue.Remove);
            ListEnemyYellow.RemoveAll(ListEnemyYellow.Remove);
            ListBall.RemoveAll(ListBall.Remove);
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        CurrentLevel.Draw();

        Paddle.Draw(gameTime);
        Ball.Draw(gameTime);

        foreach (Ball b in ListBall)
        {
            _spriteBatch.Draw(b.SpriteTexture, b.Position, Color.White);
        }

        foreach (Bonus b in ListBonus)
        {
            _spriteBatch.Draw(b.SpriteTexture, b.Position, Color.White);
        }

        foreach (EnemyBlue eBlue in ListEnemyBlue)
        {
            eBlue.Draw(gameTime);
        }

        foreach (EnemyYellow eYellow in ListEnemyYellow)
        {
            eYellow.Draw(gameTime);
        }

        _spriteBatch.DrawString(
            _assets.GetFont("TitleFont"),
            CurrentScore.ToString(),
            new Vector2(10, _screenSize.height - 32),
            Color.White);
    }
}
