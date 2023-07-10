using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Color = Microsoft.Xna.Framework.Color;

namespace ProjectBreakout;

internal class SceneGameplay : Scene
{
    // LEVEL
    private LevelManager CurrentLevel { get; set; }

    // SPRITES
    private Paddle Paddle { get; set; }
    private Ball Ball { get; set; }

    //LIST
    private List<Ball> ListBall { get; set; }
    private List<Bonus> ListBonus { get; set; }
    private List<Bonus> ListActiveBonus { get; set; }
    private List<EnemyBlue> ListEnemyBlue { get; set; }
    private List<EnemyYellow> ListEnemyYellow { get; set; }
    private string[] BonusType { get; set; }

    // SCORE
    private int CurrentScore { get; set; }

    // SOUNDS
    private Song SongGameplay { get; set; }    
    
    // TIMERS
    private float TimerCreateEnemy { get; set; }
    private float TimerBonus { get; set; }

    // BOOL
    private bool InvertedCommands { get; set; }
    private bool FastBall { get; set; }
    private bool SlowBall { get; set; }
    private bool MultiBall { get; set; }
    private bool StickyBall { get; set; }
    private bool CollisionBrick { get; set; }
    private bool CollisionPaddle { get; set; }

    // OTHER
    private Vector2 OldSpeedBall { get; set; }
    private Random Random { get; set; }

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
    }

    public override void Load()
    {
        base.Load();

        StickyBall = true;
        CollisionBrick = false;
        CollisionPaddle = true;

        CurrentScore = 0;
        Random = new();

        TimerBonus = 0;
        TimerCreateEnemy = Random.Next(10, 21);

        SongGameplay = _assets.GetSong("moonlight");

        MediaPlayer.Play(SongGameplay);
        MediaPlayer.IsRepeating = true;

        OldKeyboardState = Keyboard.GetState();

        // LOAD LEVEL
        CurrentLevel.LoadLevel(1);
        Debug.WriteLine("Author = " + CurrentLevel.Level.Author);
        Debug.WriteLine("Lines = " + CurrentLevel.Level.Lines);
        Debug.WriteLine("Columns = " + CurrentLevel.Level.Columns);

        // LOAD SPRITES
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
    }

    public override void Unload()
    {
        MediaPlayer.Stop();
        ScoreManager.SaveScore();
        base.Unload();
    }

    public void CreateBonus(float pX, float pY, int pWidth)
    {
        int probaBonus = Random.Next(0, 2);
        Debug.WriteLine("Proba = " + probaBonus);

        if (probaBonus != 0 && TimerBonus <= 0 && !MultiBall && !FastBall && !SlowBall)
        {
            Bonus randomBonus = new(BonusType[Random.Next(0, BonusType.Length)]);
            randomBonus.SetPosition(pX + pWidth / 2 - randomBonus.Width / 2, pY);

            ListBonus.Add(randomBonus);
        }
    }

    public void CreateEnemies(GameTime gameTime)
    {
        TimerCreateEnemy -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        int probaEnemy = Random.Next(0, 2);

        if (TimerCreateEnemy <= 0 && ListEnemyBlue.Count < 5)
        {
            if (probaEnemy == 0)
            {
                EnemyBlue enemyBlue = new("Enemy_1");
                enemyBlue.SetPosition(Random.Next(0, _screenSize.width - enemyBlue.Width), 0);
                ListEnemyBlue.Add(enemyBlue);
            }
            else if (probaEnemy == 1)
            {
                EnemyYellow enemyYellow = new("Enemy_2");
                enemyYellow.SetPosition(Random.Next(0, _screenSize.width - enemyYellow.Width), 0);
                ListEnemyYellow.Add(enemyYellow);
            }

            TimerCreateEnemy = Random.Next(10, 21);
        }
    }

    public void ChangeColorBall(Ball pBall)
    {
        if (Paddle.PColor == Paddle.PaddleColor.Blue)
        {
            pBall.ChangeColor(Ball.BallColor.Blue);
        }
        else if (Paddle.PColor == Paddle.PaddleColor.Red)
        {
            pBall.ChangeColor(Ball.BallColor.Red);
        }
        else if (Paddle.PColor == Paddle.PaddleColor.Yellow)
        {
            pBall.ChangeColor(Ball.BallColor.Yellow);
        }
    }

    public override void Update(GameTime gameTime)
    {
        foreach (Background background in Backgrounds)
        {
            background.Update(gameTime);
        }

        NewKeyboardState = Keyboard.GetState();

        if (NewKeyboardState.IsKeyDown(Keys.Space) &&
            OldKeyboardState != NewKeyboardState)
        {
            StickyBall = false;
            CollisionPaddle = false;
        }

        OldKeyboardState = NewKeyboardState;

        // UPDATE PADDLE
        if (!InvertedCommands)
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

        // UPDATE BALL
        Ball.Move();
        Ball.Update(gameTime);

        if (StickyBall)
        {
            Ball.StickyBall(Paddle.Position.X, Paddle.Position.Y, Paddle.Width);
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
            ChangeColorBall(Ball);
        }

        // UPDATE BRICKS
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

                if (CollisionBrick)
                {
                    if (brick.Color == ball.Color)
                    {
                        brick.Life = 0;
                    }
                    else
                    {
                        brick.Life--;
                    }

                    if (brick.Life >= 1)
                    {
                        CurrentScore = ScoreManager.IncrementScore(10);
                    }
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

            if (CollisionBrick)
            {
                if (brick.Color == Ball.Color || Ball.Color == "Big")
                {
                    brick.Life = 0;
                }
                else
                {
                    brick.Life--;
                }

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
                CreateBonus(brick.Position.X, brick.Position.Y, brick.Width);
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
                TimerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Ball.ChangeColor(Ball.BallColor.Big);

                if (TimerBonus >= 10 || StickyBall)
                {
                    Ball.ChangeColor(Ball.BallColor.Blue);
                    TimerBonus = 0;
                    ListActiveBonus.Remove(bonus);
                }
            }
            else if (bonus.NameImage == "big_bar")
            {
                TimerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Paddle.ChangeLenght(Paddle.PaddleLenght.XXLarge);

                if (TimerBonus >= 10)
                {
                    Paddle.ChangeLenght(Paddle.PaddleLenght.Large);
                    TimerBonus = 0;
                    ListActiveBonus.Remove(bonus);
                }
            }
            else if (bonus.NameImage == "fast_ball")
            {
                OldSpeedBall = new Vector2(Math.Abs(Ball.Speed.X), Math.Abs(Ball.Speed.Y));
                Debug.WriteLine("Old Speed ball = " + OldSpeedBall);
                Ball.FastBall();
                FastBall = true;
                ListActiveBonus.Remove(bonus);
            }
            else if (bonus.NameImage == "inverted_commands")
            {
                InvertedCommands = true;

                TimerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (TimerBonus >= 10)
                {
                    InvertedCommands = false;
                    TimerBonus = 0;
                    ListActiveBonus.Remove(bonus);
                }
            }
            else if (bonus.NameImage == "multiball")
            {
                MultiBall = true;

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
                        speed_x = Random.Next(-4, 4);
                    } while (speed_x < 2 && speed_x > -2);

                    do
                    {
                        speed_y = Random.Next(-4, 4);
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
                SlowBall = true;
                ListActiveBonus.Remove(bonus);
            }
            else if (bonus.NameImage == "small_bar")
            {
                TimerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Paddle.ChangeLenght(Paddle.PaddleLenght.Small);

                if (TimerBonus >= 10)
                {
                    Paddle.ChangeLenght(Paddle.PaddleLenght.Large);
                    TimerBonus = 0;
                    ListActiveBonus.Remove(bonus);
                }
            }
        }

        if (FastBall)
        {
            TimerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TimerBonus >= 10)
            {
                Ball.RestoreSpeed(OldSpeedBall.X, OldSpeedBall.Y);

                TimerBonus = 0;
                FastBall = false;
            }
        }

        if (SlowBall)
        {
            TimerBonus += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TimerBonus >= 10)
            {
                Ball.RestoreSpeed(OldSpeedBall.X, OldSpeedBall.Y);

                TimerBonus = 0;
                SlowBall = false;
            }
        }

        // UPDATE MULTIBALL
        if (ListBall.Count == 0)
        {
            MultiBall = false;
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
                ChangeColorBall(newBall);
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

        // UPDATE ENEMY

        CreateEnemies(gameTime);

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
            TimerCreateEnemy = Random.Next(10, 31);
            TimerBonus = 5;

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

        foreach (Ball ball in ListBall)
        {
            _spriteBatch.Draw(ball.SpriteTexture, ball.Position, Color.White);
        }

        foreach (Bonus bonus in ListBonus)
        {
            _spriteBatch.Draw(bonus.SpriteTexture, bonus.Position, Color.White);
        }

        foreach (EnemyBlue enemyBlue in ListEnemyBlue)
        {
            enemyBlue.Draw(gameTime);
        }

        foreach (EnemyYellow enemyYellow in ListEnemyYellow)
        {
            enemyYellow.Draw(gameTime);
        }

        _spriteBatch.DrawString(
            _assets.GetFont("TitleFont"),
            CurrentScore.ToString(),
            new Vector2(10, _screenSize.height - 32),
            Color.White);
    }
}
